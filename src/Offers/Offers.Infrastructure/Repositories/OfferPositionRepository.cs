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

    public async Task<OfferPosition?> Insert(OfferPosition offerPosition, CancellationToken ct)
    {
        var command = InsertBaseCommand(offerPosition, ct);

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