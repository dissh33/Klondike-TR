using App.Metrics;
using Serilog;
using System.Data;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Dapper;
using Offers.Infrastructure.Logging;

namespace Offers.Infrastructure.Repositories;

public class OfferPositionRepository : BaseRepository<OfferPosition>, IOfferPositionRepository
{
    public OfferPositionRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics)
        : base(transaction, logger, metrics)
    {
    }

    public async Task<OfferPosition?> GetById(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<OfferPosition>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<OfferPosition>> GetByCollection(Guid offerId, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE offer_id = @offerId",
            parameters: new { offerId },
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<OfferPosition>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<OfferPosition?> Insert(OfferPosition offerPosition, CancellationToken ct)
    {
        var command = InsertBaseCommand(offerPosition, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<IEnumerable<OfferPosition>> BulkInsert(IEnumerable<OfferPosition> offerPositions, CancellationToken ct)
    {
        var insertColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var subSql = string.Join(", ", 
            offerPositions.Select(position => 
                $"('{position.Id.Value}', " +
                $"'{position.OfferId?.Value}', " +
                $"'{position.PriceRate}', " +
                $"'{position.WithTrader}', " +
                $"'{position.Message}', " +
                $"'{position.Type}', " +
                $"'{position.CreateDate}')"));

        var sql = $"INSERT INTO {SCHEMA_NAME}.{TableName} ({insertColumns}) VALUES {subSql} RETURNING offer_id;";

        var command = new CommandDefinition(
            commandText: sql,
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var collectionId = await Logger.DbCall(query, command, Metrics);

        return await GetByCollection(collectionId, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(command);

        return await Logger.DbCall(query, command, Metrics);
    }
}