using System.Data;
using App.Metrics;
using Dapper;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Infrastructure.Logging;
using Serilog;

namespace Offers.Infrastructure.Repositories;

public class OfferRepository : BaseRepository<Offer>, IOfferRepository
{
    public OfferRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics) 
        : base(transaction, logger, metrics)
    {
    }

    public async Task<Offer?> GetById(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<Offer>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<Offer?> Insert(Offer offer, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var insertColumns = $"({selectColumns}, expression) VALUES ({string.Join(", ", GetColumns().Select(e => "@" + e))}, CAST(@Expression AS json))";

        var sql = $"INSERT INTO {SCHEMA_NAME}.{TableName} {insertColumns} RETURNING id";
        
        var command = new CommandDefinition(
            commandText: sql,
            parameters: offer,
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);
        
        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(command);

        return await Logger.DbCall(query, command, Metrics);
    }
}