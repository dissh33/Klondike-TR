using Dapper;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Infrastructure.Logging;
using Offers.Infrastructure.Metrics;
using Serilog;
using System.Data;

namespace Offers.Infrastructure.Repositories;

public class OfferItemRepository(IDbTransaction transaction, ILogger logger, DbMetrics metrics) 
    : BaseRepository<OfferItem>(transaction, logger, metrics), IOfferItemRepository
{
    public async Task<OfferItem?> GetById(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<OfferItem>(command);

        return await DbCall(query, command);
    }

    public async Task<IEnumerable<OfferItem>> GetByPosition(Guid offerPositionId, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var command = CreateCommand(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE offer_position_id = @offerPositionId",
            parameters: new { offerPositionId },
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<OfferItem>(command);

        return await DbCall(query, command);
    }

    public async Task<IEnumerable<OfferItem>> GetByPositions(IEnumerable<Guid> offerPositionIds, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var offerPositionIdsSql = string.Join(", ", offerPositionIds.Select(id => $"'{id}'"));

        var command = CreateCommand(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE offer_position_id IN ({offerPositionIdsSql})",
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<OfferItem>(command);

        return await DbCall(query, command);
    }

    public async Task<OfferItem?> Insert(OfferItem offerItem, CancellationToken ct)
    {
        var command = InsertBaseCommand(offerItem, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<IEnumerable<OfferItem>> BulkInsert(IEnumerable<OfferItem> offerItems, CancellationToken ct)
    {
        var insertColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var valuesSql = string.Join(", ",
            offerItems.Select(item =>
                $"('{item.Id.Value}', " +
                $"'{item.OfferPositionId?.Value}', " +
                $"'{item.TradableItemId}', " +
                $"'{item.Amount}', " +
                $"'{(int)item.Type}', " +
                $"'{item.CreateDate}')"));

        var sql = $"INSERT INTO {SCHEMA_NAME}.{TableName} ({insertColumns}) VALUES {valuesSql} RETURNING offer_position_id;";

        var command = CreateCommand(
            commandText: sql,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var positionId = await DbCall(query, command);

        return await GetByPosition(positionId, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(command);

        return await DbCall(query, command);
    }
}