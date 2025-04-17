using Harmony.API.Common.Errors;
using Harmony.Application;
using Harmony.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NLog.Extensions.Logging;

namespace Harmony.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSwaggerGen()    // Add Swagger
            .AddApplication()   // Add Services from Application Layer (MediatR, AutoMapper, Validators)
            .AddInfrastructure(configuration)  // Add Services from Infrastructure Layer (DbContext, Identity, Auth, Repositories, Services)
            .AddLogging(configuration)    // Add Logger
            .AddCors()    // Add CORS policy to allow client origin in localhost
            .AddAuthorization()
            .AddCustomProblemDetailsFactory() // Add the custom ProblemDetailsFactory declared in Error folder
            .AddControllers();

        return services;
    }

    public static IServiceCollection AddCustomProblemDetailsFactory(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, HarmonyProblemDetailsFactory>();

        return services;
    }

    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Logger
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddNLog(configuration);
        });

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        // Add CORS policy to allow client origin in localhost
        services.AddCors(policy =>
        {
            policy.AddPolicy("HarmonyUI", builder => builder.WithOrigins("https://localhost:7222")
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        });

        return services;
    }
}
