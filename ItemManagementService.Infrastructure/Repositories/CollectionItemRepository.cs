using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionItemRepository : BaseRepository<CollectionItem>, ICollectionItemRepository
{
    private readonly NpgsqlConnection _connection;

    public CollectionItemRepository(IConfiguration configuration) : base(configuration)
    {
        _connection = new NpgsqlConnection(ConnectionString);
    }

    public async Task<CollectionItem> GetById(int id, CancellationToken ct)
    {
        var cmd = GetByIdCommand(id, ct);

        return await _connection.QueryFirstOrDefaultAsync<CollectionItem>(cmd);
    }

    public async Task<IEnumerable<CollectionItem>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllCommand(ct);

        return await _connection.QueryAsync<CollectionItem>(cmd);
    }

    public async Task<CollectionItem> Insert(CollectionItem collectionItem, CancellationToken ct)
    {
        var cmd = InsertCommand(collectionItem, ct);

        var id = await _connection.ExecuteScalarAsync<int>(cmd);

        return await GetById(id, ct);
    }

    public async Task<CollectionItem> Update(CollectionItem collectionItem, CancellationToken ct)
    {
        var cmd = UpdateCommand(collectionItem, ct);

        var id = await _connection.ExecuteScalarAsync<int>(cmd);

        return await GetById(id, ct);
    }

    public async Task Delete(int id, CancellationToken ct)
    {
        var cmd = DeleteCommand(id, ct);

        await _connection.ExecuteAsync(cmd);
    }
}
