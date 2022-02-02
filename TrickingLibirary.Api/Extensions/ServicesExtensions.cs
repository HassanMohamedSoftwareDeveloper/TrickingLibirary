using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.BackgroundServices.VideoEditing;
using TrickingLibirary.Domain.Interfaces;
using TrickingLibirary.Infrastructure.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Models;

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
        services.AddSingleton<VideoManager>();
        return services;
    }
    public static IServiceCollection RegisterIdentityServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddDbContext<IdentityDbContext>(config =>
        config.UseInMemoryDatabase("DevIdentity"));
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
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
            config.LoginPath = "/account/login";
        });

        var identityServerBuilder = services.AddIdentityServer();
        identityServerBuilder.AddAspNetIdentity<IdentityUser>();

        if (env.IsDevelopment())
        {
            identityServerBuilder.AddInMemoryIdentityResources(new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            });

            identityServerBuilder.AddInMemoryClients(new Client[]
            {
                new Client
                {
                    ClientId="web-client",
                    AllowedGrantTypes=GrantTypes.Code,
                    RedirectUris=new []{ "http://localhost:3000" },
                    PostLogoutRedirectUris=new []{ "http://localhost:3000" },
                    AllowedCorsOrigins=new []{ "http://localhost:3000" },


                    RequirePkce=true,
                    AllowAccessTokensViaBrowser=true,

                    RequireConsent=false,
                    RequireClientSecret=false,
                }
            });

            identityServerBuilder.AddDeveloperSigningCredential();
        }
        return services;
    }
}
