using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Domain.Interfaces;
using TrickingLibirary.Infrastructure.Data;

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

    public static IServiceCollection RegisterDatabase(this IServiceCollection services)
    {
        services.AddDbContext<IDbContext, AppDbContext>(options => options.UseInMemoryDatabase("Dev"));
        return services;
    }
}
