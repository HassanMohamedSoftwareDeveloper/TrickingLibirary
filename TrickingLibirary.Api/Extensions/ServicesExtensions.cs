using System.Threading.Channels;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.BackgroundServices.VideoEditing;
using TrickingLibirary.Api.Helpers;
using TrickingLibirary.Api.Settings;
using TrickingLibirary.Domain.Interfaces;
using TrickingLibirary.Infrastructure.Data;

namespace TrickingLibirary.Api.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection RegisterWebServices(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddControllers();
        services.AddCors(options => options.AddPolicy("All", build => build
           .AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()
          ));
        return services;
    }
    public static IServiceCollection RegisterDatabase(this IServiceCollection services)
    {
        services.AddDbContext<IDbContext, AppDbContext>(options => options.UseInMemoryDatabase("Dev"));
        return services;
    }
    public static IServiceCollection RegisterVideoManagerServices(this IServiceCollection services)
    {
        services.AddHostedService<VideoEditingBackgroundService>();
        services.AddSingleton(_ => Channel.CreateUnbounded<EditVideoMessage>());
        return services;
    }
    public static IServiceCollection RegisterFileManager(this IServiceCollection services, IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(nameof(FileSettings));
        var settings = settingsSection.Get<FileSettings>();
        services.Configure<FileSettings>(settingsSection);
        if (settings.Provider.Equals(Tricking_LibiraryConstants.Providers.Local))
            services.AddSingleton<IFileManager, FileManagerLocal>();
        else if (settings.Provider.Equals(Tricking_LibiraryConstants.Providers.S3))
            throw new NotImplementedException();
        else
            throw new Exception($"Invalid File Manager Provider {settings.Provider}");
        return services;
    }
    public static IServiceCollection RegisterIdentityServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddDbContext<IdentityDbContext>(config =>
        config.UseInMemoryDatabase("DevIdentity"));

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            if (env.IsDevelopment())
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }
            else
            {
                //configure for Production
            }
        })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(config =>
        {
            //// add an instance of the patched manager to the options:
            config.CookieManager = new ChunkingCookieManager();

            config.Cookie.HttpOnly = true;
            config.Cookie.SameSite = SameSiteMode.None;
            config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            config.LoginPath = "/account/login";
            config.LogoutPath = "/api/auth/logout";
        });

        var identityServerBuilder = services.AddIdentityServer();
        identityServerBuilder.AddAspNetIdentity<IdentityUser>();

        if (env.IsDevelopment())
        {
            identityServerBuilder.AddInMemoryIdentityResources(new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(Tricking_LibiraryConstants.IdentityResources.RoleScope,
                new[]{Tricking_LibiraryConstants.Claims.Role }),
            });
            identityServerBuilder.AddInMemoryApiScopes(new ApiScope[]
            {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName,new []{
                    JwtClaimTypes.PreferredUserName,
                    Tricking_LibiraryConstants.Claims.Role})
            });

            identityServerBuilder.AddInMemoryClients(new Client[]
            {
                new Client
                {
                    ClientId="web-client",
                    AllowedGrantTypes=GrantTypes.Code,
                    RedirectUris=new []
                    {
                        "https://localhost:3000/oidc/sign-in-callback.html" ,
                        "https://localhost:3000/oidc/sign-in-silent-callback.html"
                    },
                    PostLogoutRedirectUris=new []{ "https://localhost:3000" },
                    AllowedCorsOrigins=new []{ "https://localhost:3000" },

                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.LocalApi.ScopeName,
                        Tricking_LibiraryConstants.IdentityResources.RoleScope
                    },

                    RequirePkce=true,
                    AllowAccessTokensViaBrowser=true,

                    RequireConsent=false,
                    RequireClientSecret=false,
                }
            });

            identityServerBuilder.AddDeveloperSigningCredential();
        }

        services.AddLocalApiAuthentication();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Tricking_LibiraryConstants.Policies.Mod, policy =>
            {
                var is4Policy = options.GetPolicy(IdentityServerConstants.LocalApi.PolicyName);
                policy.Combine(is4Policy);

                policy.RequireClaim(Tricking_LibiraryConstants.Claims.Role, Tricking_LibiraryConstants.Roles.Mod);
            });
        });
        return services;
    }
}
