using Items.Application.Contracts;
using Items.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Items.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}
