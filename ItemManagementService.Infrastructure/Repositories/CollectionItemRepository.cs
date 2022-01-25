using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionItemRepository : Repository<CollectionItem>, ICollectionItemRepository
{
    private readonly NpgsqlConnection _connection;

    public CollectionItemRepository(string connectionString) 
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public async Task<CollectionItem> GetById(Guid id, CancellationToken ct)
    {
        var cmd = GetByIdBaseCommand(id, ct);

        return await _connection.QueryFirstOrDefaultAsync<CollectionItem>(cmd);
    }

    public async Task<IEnumerable<CollectionItem>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        return await _connection.QueryAsync<CollectionItem>(cmd);
    }

    public async Task<CollectionItem> Insert(CollectionItem collectionItem, CancellationToken ct)
    {
        var cmd = InsertBaseCommand(collectionItem, ct);

        var id = await _connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task<CollectionItem> Update(CollectionItem collectionItem, CancellationToken ct)
    {
        var cmd = UpdateBaseCommand(collectionItem, ct);

        var id = await _connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var cmd = DeleteBaseCommand(id, ct);

        await _connection.ExecuteAsync(cmd);
    }

    override public void Dispose()
    {
        _connection.Close();
        _connection.DisposeAsync();
        base.Dispose();
    }
}
