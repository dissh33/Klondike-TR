using System.Data;
using App.Metrics;
using Dapper;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Infrastructure.Logging;
using Serilog;

namespace Offers.Infrastructure.Repositories;

public class OfferItemRepository : BaseRepository<OfferItem>, IOfferItemRepository
{
    public OfferItemRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics)
        : base(transaction, logger, metrics)
    {
    }

    public async Task<OfferItem?> GetById(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<OfferItem>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<OfferItem>> GetByPosition(Guid positionId, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE position_id = @positionId",
            parameters: new { positionId },
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<OfferItem>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<OfferItem?> Insert(OfferItem offerItem, CancellationToken ct)
    {
        var command = InsertBaseCommand(offerItem, ct);

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