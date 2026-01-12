using Dapper;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Infrastructure.Metrics;
using Serilog;
using System.Data;

namespace Offers.Infrastructure.Repositories;

public class OfferPositionRepository(IDbTransaction transaction, ILogger logger, DbMetrics metrics) 
    : BaseRepository<OfferPosition>(transaction, logger, metrics), IOfferPositionRepository
{
    public async Task<OfferPosition?> GetById(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<OfferPosition>(command);

        return await DbCall(query, command);
    }

    public async Task<IEnumerable<OfferPosition>> GetByOffer(Guid offerId, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var command = CreateCommand(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE offer_id = @offerId",
            parameters: new { offerId },
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<OfferPosition>(command);

        return await DbCall(query, command);
    }

    public async Task<IEnumerable<OfferPosition>> GetByOffers(IEnumerable<Guid> offerIds, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var offerIdsSql = string.Join(", ", offerIds.Select(id => $"'{id}'"));

        var command = CreateCommand(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE offer_id IN ({offerIdsSql})",
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<OfferPosition>(command);

        return await DbCall(query, command);
    }

    public async Task<OfferPosition?> Insert(OfferPosition offerPosition, CancellationToken ct)
    {
        var command = InsertBaseCommand(offerPosition, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<IEnumerable<OfferPosition>> BulkInsert(IEnumerable<OfferPosition> offerPositions, CancellationToken ct)
    {
        var insertColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var valuesSql = string.Join(", ", 
            offerPositions.Select(position => 
                $"('{position.Id.Value}', " +
                $"'{position.OfferId?.Value}', " +
                $"'{position.PriceRate}', " +
                $"'{position.WithTrader}', " +
                $"'{position.Message}', " +
                $"'{(int)position.Type}', " +
                $"'{position.CreateDate}')"));

        var sql = $"INSERT INTO {SCHEMA_NAME}.{TableName} ({insertColumns}) VALUES {valuesSql} RETURNING offer_id;";

        var command = CreateCommand(
            commandText: sql,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var offerId = await DbCall(query, command);

        return await GetByOffer(offerId, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(command);

        return await DbCall(query, command);
    }
}