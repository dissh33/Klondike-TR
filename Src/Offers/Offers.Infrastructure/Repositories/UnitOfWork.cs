using System.Data;
using App.Metrics;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Offers.Application.Contracts;
using Serilog;

namespace Offers.Infrastructure.Repositories;

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
        //_metrics.Measure.Counter.Increment(MetricsRegistry.DbConnectionsCounter); //TODO: Metrics

        _transactionId = Guid.NewGuid();
        _transaction = _connection.BeginTransaction();
        _logger.Information("Begin transaction {@id}", _transactionId);
    }

    private IOfferRepository? _offerRepository;
    public IOfferRepository OfferRepository => _offerRepository ??= new OfferRepository(_transaction, _logger, _metrics);

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
        _offerRepository?.Dispose();
    }
}
