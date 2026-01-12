using Items.Application.Contracts;
using Items.Infrastructure.Metrics;
using Items.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Items.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<DbMetrics>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}
