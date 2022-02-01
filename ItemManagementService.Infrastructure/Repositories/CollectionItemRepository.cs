using System.Data;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionItemBaseRepository : BaseRepository<CollectionItem>, ICollectionItemRepository
{    public CollectionItemBaseRepository(IDbTransaction transaction, ILogger logger)
        : base(transaction, logger)
    {

    }

    public async Task<CollectionItem> GetById(Guid id, CancellationToken ct)
    {
        var cmd = GetByIdBaseCommand(id, ct);

        return await Connection.QueryFirstOrDefaultAsync<CollectionItem>(cmd);
    }

    public async Task<IEnumerable<CollectionItem>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        return await Connection.QueryAsync<CollectionItem>(cmd);
    }

    public async Task<CollectionItem> Insert(CollectionItem collectionItem, CancellationToken ct)
    {
        var cmd = InsertBaseCommand(collectionItem, ct);

        var id = await Connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task<CollectionItem> Update(CollectionItem collectionItem, CancellationToken ct)
    {
        var cmd = UpdateBaseCommand(collectionItem, ct);

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
