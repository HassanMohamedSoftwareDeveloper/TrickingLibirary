using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.BackgroundServices.VideoEditing;
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
        services.AddSingleton<VideoManager>();
        return services;
    }
}
