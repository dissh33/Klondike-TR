using System.Data;
using ItemManagementService.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ILogger _logger;
    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;
    private Guid _transactionId;

    public UnitOfWork(IConfiguration configuration, ILogger logger)
    {
        _logger = logger;

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
            
        _transactionId = Guid.NewGuid();
        _transaction = _connection.BeginTransaction();
        _logger.Information("Begin transaction {@id}", _transactionId);
    }

    private IIconRepository? _iconRepository;
    private IMaterialRepository? _materialRepository;
    private ICollectionRepository? _collectionRepository;
    private ICollectionItemRepository? _collectionItemRepository;

    public IIconRepository IconRepository => _iconRepository ??= new IconBaseRepository(_transaction, _logger);
    public IMaterialRepository MaterialRepository => _materialRepository ??= new MaterialBaseRepository(_transaction, _logger);
    public ICollectionRepository CollectionRepository => _collectionRepository ??= new CollectionBaseRepository(_transaction, _logger);
    public ICollectionItemRepository CollectionItemRepository => _collectionItemRepository ??= new CollectionItemBaseRepository(_transaction, _logger);

    public void Commit()
    {
        try
        {

            _transaction.Commit();

            _logger.Information("Commit transaction {@id}", _transactionId);
        }
        catch
        {
            _transaction.Rollback();
            _logger.Information("Rollback transaction {@id}", _transactionId);
            throw;
        }
        finally
        {
            _transaction.Dispose();

            _transactionId = Guid.NewGuid();
            _transaction = _connection.BeginTransaction();
            _logger.Information("Begin transaction {@id}", _transactionId);

            ResetRepositories();
        }
    }

    public void Rollback()
    {
        _transaction.Rollback();
        _logger.Information("Rollback transaction {@id}", _transactionId);

        _transaction.Dispose();

        _transactionId = Guid.NewGuid();
        _transaction = _connection.BeginTransaction();
        _logger.Information("Begin transaction {@id}", _transactionId);

        ResetRepositories();
    }

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
            ResetRepositories();
            _logger.Information("Dispose transaction {@id}", _transactionId);
        }
        _disposed = true;
    }

    private void ResetRepositories()
    {
        _iconRepository?.Dispose();
        _materialRepository?.Dispose();
        _collectionRepository?.Dispose();
        _collectionItemRepository?.Dispose();
    }
}
