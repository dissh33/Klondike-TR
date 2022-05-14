using ItemManagementService.Application.Contracts;
using ItemManagementService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ItemManagementService.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}
