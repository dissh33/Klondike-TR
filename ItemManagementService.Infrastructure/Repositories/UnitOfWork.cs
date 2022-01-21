using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ItemManagementService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    protected readonly string ConnectionString;

    public UnitOfWork(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IGenericRepository<Icon>? _iconRepository;
    private IGenericRepository<Material>? _materialRepository;
    private IGenericRepository<Collection>? _collectionRepository;
    private IGenericRepository<CollectionItem>? _collectionItemRepository;

    public IGenericRepository<Icon> IconRepository => _iconRepository ??= new IconRepository(ConnectionString);
    public IGenericRepository<Material> MaterialRepository => _materialRepository ??= new MaterialRepository(ConnectionString);
    public IGenericRepository<Collection> CollectionRepository => _collectionRepository ??= new CollectionRepository(ConnectionString);
    public IGenericRepository<CollectionItem> CollectionItemRepository => _collectionItemRepository ??= new CollectionItemRepository(ConnectionString);
   
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed && disposing)
        {
            _iconRepository?.Dispose();
            _materialRepository?.Dispose();
            _collectionRepository?.Dispose();
            _collectionItemRepository?.Dispose();
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
