using System.Data;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
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

        return await Connection.QueryAsync<Collection>(cmd);
    }

    public async Task<Collection> Insert(Collection collection, CancellationToken ct)
    {
        var cmd = InsertBaseCommand(collection, ct);

        var id = await Connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task<Collection> Update(Collection collection, CancellationToken ct)
    {
        var cmd = UpdateBaseCommand(collection, ct);

        var id = await Connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var cmd = DeleteBaseCommand(id, ct);

        await Connection.ExecuteAsync(cmd);
    }

    override public void Dispose()
    {
        Connection?.Close();
        Connection?.Dispose();
        base.Dispose();
    }
}
