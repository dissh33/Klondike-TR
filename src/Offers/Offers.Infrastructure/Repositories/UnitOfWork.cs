using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Offers.Application.Contracts;
using Offers.Infrastructure.Metrics;
using Serilog;

namespace Offers.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ILogger _logger;
    private readonly DbMetrics _metrics;

    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;
    private Guid _transactionId;

    public UnitOfWork(IConfiguration configuration, ILogger logger, DbMetrics metrics)
    {
        _logger = logger;
        _metrics = metrics;

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
        _metrics.DbConnectionsCounter.Add(1);

        _transactionId = Guid.NewGuid();
        _transaction = _connection.BeginTransaction();
        _logger.Information("Begin transaction {@id}", _transactionId);
    }

    private IOfferRepository? _offerRepository;
    private IOfferPositionRepository? _offerPositionRepository;
    private IOfferItemRepository? _offerItemRepository;

    public IOfferRepository OfferRepository => _offerRepository ??= new OfferRepository(_transaction, _logger, _metrics);
    public IOfferPositionRepository OfferPositionRepository => _offerPositionRepository ??= new OfferPositionRepository(_transaction, _logger, _metrics);
    public IOfferItemRepository OfferItemRepository => _offerItemRepository ??= new OfferItemRepository(_transaction, _logger, _metrics);

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

            StartNewTransaction();
            ResetRepositories();
        }
    }

    public void Rollback()
    {
        _transaction.Rollback();
        _logger.Information("Rollback transaction {@id}", _transactionId);

        _transaction.Dispose();

        StartNewTransaction();
        ResetRepositories();
    }

    private void StartNewTransaction()
    {
        _transactionId = Guid.NewGuid();
        _transaction = _connection.BeginTransaction();
        _logger.Information("Begin transaction {@id}", _transactionId);
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
            _connection.Dispose();

            _disposed = true;
        }
    }

    private void ResetRepositories()
    {
        _offerRepository?.Dispose();
    }
}
