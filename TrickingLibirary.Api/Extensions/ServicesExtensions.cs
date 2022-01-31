using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.BackgroundServices;
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

    public static IServiceCollection RegisterBackgroundService(this IServiceCollection services)
    {
        services.AddHostedService<VideoEditingBackgroundService>();
        return services;
    }
    public static IServiceCollection RegisterChannel(this IServiceCollection services)
    {
        services.AddSingleton(_ => Channel.CreateUnbounded<EditVideoMessage>());
        return services;
    }
}
