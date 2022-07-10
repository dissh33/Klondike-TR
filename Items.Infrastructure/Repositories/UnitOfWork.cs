using System.Data;
using System.Runtime.CompilerServices;
using App.Metrics;
using Items.Application.Contracts;
using Items.Infrastructure.Metrics;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Items.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ILogger _logger;
    private readonly IMetrics _metrics;

    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;
    private Guid _transactionId;

    public UnitOfWork(IConfiguration configuration, ILogger logger, IMetrics metrics)
    {
        _logger = logger;
        _metrics = metrics;

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
        _metrics.Measure.Counter.Increment(MetricsRegistry.DbConnectionsCounter);

        _transactionId = Guid.NewGuid();
        _transaction = _connection.BeginTransaction();
        _logger.Information("Begin transaction {@id}", _transactionId);
    }

    private IIconRepository? _iconRepository;
    private IMaterialRepository? _materialRepository;
    private ICollectionRepository? _collectionRepository;
    private ICollectionItemRepository? _collectionItemRepository;

    public IIconRepository IconRepository => _iconRepository ??= new IconRepository(_transaction, _logger, _metrics);
    public IMaterialRepository MaterialRepository => _materialRepository ??= new MaterialRepository(_transaction, _logger, _metrics);
    public ICollectionRepository CollectionRepository => _collectionRepository ??= new CollectionRepository(_transaction, _logger, _metrics);
    public ICollectionItemRepository CollectionItemRepository => _collectionItemRepository ??= new CollectionItemRepository(_transaction, _logger, _metrics);

    public void Commit(bool createNew = true)              
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

    private bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            ResetRepositories();

            _transaction.Dispose();
            _logger.Information("Dispose transaction {@id}", _transactionId);
            
            _disposed = true;
        }        
    }

    private void ResetRepositories()
    {
        _iconRepository?.Dispose();
        _materialRepository?.Dispose();
        _collectionRepository?.Dispose();
        _collectionItemRepository?.Dispose();
    }
}
