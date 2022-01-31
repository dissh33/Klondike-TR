using ItemManagementService.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ILogger _logger;
    private readonly string _connectionString;

    public UnitOfWork(IConfiguration configuration, ILogger logger)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IIconRepository? _iconRepository;
    private IMaterialRepository? _materialRepository;
    private ICollectionRepository? _collectionRepository;
    private ICollectionItemRepository? _collectionItemRepository;

    public IIconRepository IconRepository => _iconRepository ??= new IconRepository(_connectionString, _logger);
    public IMaterialRepository MaterialRepository => _materialRepository ??= new MaterialRepository(_connectionString);
    public ICollectionRepository CollectionRepository => _collectionRepository ??= new CollectionRepository(_connectionString);
    public ICollectionItemRepository CollectionItemRepository => _collectionItemRepository ??= new CollectionItemRepository(_connectionString);
   
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
}
