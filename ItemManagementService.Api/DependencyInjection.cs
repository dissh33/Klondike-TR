using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace ItemManagementService.Api;

public static class DependencyInjection
{
    public static void RegisterRequestValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}