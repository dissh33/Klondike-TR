using ItemManagementService.Application.Contracts;
using Microsoft.Extensions.Configuration;

namespace ItemManagementService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    protected string ConnectionString { get; }

    public UnitOfWork(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IIconRepository? _iconRepository;
    private IMaterialRepository? _materialRepository;
    private ICollectionRepository? _collectionRepository;
    private ICollectionItemRepository? _collectionItemRepository;

    public IIconRepository IconRepository => _iconRepository ??= new IconRepository(ConnectionString);
    public IMaterialRepository MaterialRepository => _materialRepository ??= new MaterialRepository(ConnectionString);
    public ICollectionRepository CollectionRepository => _collectionRepository ??= new CollectionRepository(ConnectionString);
    public ICollectionItemRepository CollectionItemRepository => _collectionItemRepository ??= new CollectionItemRepository(ConnectionString);
   
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _iconRepository?.Dispose();
            _materialRepository?.Dispose();
            _collectionRepository?.Dispose();
            _collectionItemRepository?.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
