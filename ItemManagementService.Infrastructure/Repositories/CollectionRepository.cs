using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionRepository : BaseRepository<Collection>, ICollectionRepository
{
    private readonly NpgsqlConnection _connection;

    public CollectionRepository(IConfiguration configuration) : base(configuration)
    {
        _connection = new NpgsqlConnection(ConnectionString);
    }

    public async Task<Collection> GetById(int id, CancellationToken ct)
    {
        var cmd = GetByIdCommand(id, ct);

        return await _connection.QueryFirstOrDefaultAsync<Collection>(cmd);
    }

    public async Task<IEnumerable<Collection>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllCommand(ct);

        return await _connection.QueryAsync<Collection>(cmd);
    }

    public async Task<Collection> Insert(Collection collection, CancellationToken ct)
    {
        var cmd = InsertCommand(collection, ct);

        var id = await _connection.ExecuteScalarAsync<int>(cmd);

        return await GetById(id, ct);
    }

    public async Task<Collection> Update(Collection collection, CancellationToken ct)
    {
        var cmd = UpdateCommand(collection, ct);

        var id = await _connection.ExecuteScalarAsync<int>(cmd);

        return await GetById(id, ct);
    }

    public async Task Delete(int id, CancellationToken ct)
    {
        var cmd = DeleteCommand(id, ct);

        await _connection.ExecuteAsync(cmd);
    }
}
