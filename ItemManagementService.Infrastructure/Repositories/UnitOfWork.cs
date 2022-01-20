using ItemManagementService.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ItemManagementService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IIconRepository? IconRepository => _serviceProvider.GetService<IIconRepository>();
        public IMaterialRepository? MaterialRepository => _serviceProvider.GetService<IMaterialRepository>();
        public ICollectionRepository? CollectionRepository => _serviceProvider.GetService<ICollectionRepository>();
        public ICollectionItemRepository? CollectionItemRepository => _serviceProvider.GetService<ICollectionItemRepository>();
}
