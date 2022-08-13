using Microsoft.Extensions.DependencyInjection;
using Offers.Application.Contracts;
using Offers.Infrastructure.Repositories;

namespace Offers.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}