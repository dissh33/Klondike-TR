using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionRepository : Repository<Collection>, ICollectionRepository
{
    private readonly NpgsqlConnection _connection;

    public CollectionRepository(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public async Task<Collection> GetById(Guid id, CancellationToken ct)
    {
        var cmd = GetByIdBaseCommand(id, ct);

        return await _connection.QueryFirstOrDefaultAsync<Collection>(cmd);
    }

    public async Task<IEnumerable<Collection>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        return await _connection.QueryAsync<Collection>(cmd);
    }

    public async Task<Collection> Insert(Collection collection, CancellationToken ct)
    {
        var cmd = InsertBaseCommand(collection, ct);

        var id = await _connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task<Collection> Update(Collection collection, CancellationToken ct)
    {
        var cmd = UpdateBaseCommand(collection, ct);

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
