namespace TrickingLibirary.Api.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddControllers();
        services.AddCors(options =>
        options.AddPolicy("All", build =>
           build.AllowAnyHeader()
           .AllowAnyOrigin()
           .AllowAnyMethod()
          ));
        return services;
    }
}
