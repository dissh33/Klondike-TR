using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class MaterialRepository : Repository<Material>, IMaterialRepository
{
    private readonly NpgsqlConnection _connection;

    public MaterialRepository(string connectionString) 
    {
        _connection = new NpgsqlConnection(connectionString);
    }
    public async Task<Material> GetById(Guid id, CancellationToken ct)
    {
        var cmd = GetByIdBaseCommand(id, ct);

        return await _connection.QueryFirstOrDefaultAsync<Material>(cmd);
    }

    public async Task<IEnumerable<Material>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        return await _connection.QueryAsync<Material>(cmd);
    }

    public async Task<Material> Insert(Material material, CancellationToken ct)
    {
        var cmd = InsertBaseCommand(material, ct);

        var id = await _connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task<Material> Update(Material material, CancellationToken ct)
    {
        var cmd = UpdateBaseCommand(material, ct);

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
