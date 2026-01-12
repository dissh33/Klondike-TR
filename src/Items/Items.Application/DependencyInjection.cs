using System.ComponentModel;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using Items.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Items.Application;

public static class DependencyInjection
{
    public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = configuration["MediatrLicenseKey"];
            cfg.RegisterServicesFromAssembly(currentAssembly);
        });

        services.AddAutoMapper(cfg => { cfg.LicenseKey = configuration["MediatrLicenseKey"]; }, currentAssembly);
        services.AddValidatorsFromAssembly(currentAssembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
    }
}