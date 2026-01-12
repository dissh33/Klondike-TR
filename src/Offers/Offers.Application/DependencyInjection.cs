using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Offers.Application.Behaviors;

namespace Offers.Application;

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
    }
}