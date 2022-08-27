using System.Reflection;
using FluentValidation;
using Items.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Items.Application;

public static class DependencyInjection
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(currentAssembly);

        services.AddValidatorsFromAssembly(currentAssembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
    }
}