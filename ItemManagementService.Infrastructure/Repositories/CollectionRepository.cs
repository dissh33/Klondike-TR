using System.Data;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using ItemManagementService.Infrastructure.Logging;
using Npgsql;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionBaseRepository : BaseRepository<Collection>, ICollectionRepository
{
    public CollectionBaseRepository(IDbTransaction transaction, ILogger logger)
        : base(transaction, logger)
    {

    }

    public async Task<Collection> GetById(Guid id, CancellationToken ct)
    {
        var cmd = GetByIdBaseCommand(id, ct);

        return await Connection.QueryFirstOrDefaultAsync<Collection>(cmd);
    }

    public async Task<IEnumerable<Collection>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        var query = async () => await Connection.QueryAsync<Collection>(cmd);

        return await Logger.DbCall(query, cmd);
    }

    public async Task<Collection> Insert(Collection collectionItem, CancellationToken ct)
    {
        var cmd = InsertBaseCommand(collectionItem, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(cmd);

        var id = await Logger.DbCall(query, cmd);

        return await GetById(id, ct);
    }

    public async Task<Collection> Update(Collection collectionItem, CancellationToken ct)
    {
        var cmd = UpdateBaseCommand(collectionItem, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(cmd);

        var id = await Logger.DbCall(query, cmd);

        return await GetById(id, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var cmd = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(cmd);

        return await Logger.DbCall(query, cmd);
    }

    override public void Dispose()
    {
        Connection?.Close();
        Connection?.Dispose();
        base.Dispose();
    }
}
