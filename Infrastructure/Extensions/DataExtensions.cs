using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DemoMinimalAPI.Infrastructure.Data;

namespace DemoMinimalAPI.Infrastructure.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite("Data Source=app.db");
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = "DemoMinimalAPI",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Kayki Letieri",
                    Email = "kaykiletieri37@gmail.com"
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
