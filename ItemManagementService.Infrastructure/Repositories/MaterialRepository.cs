using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
{
    private readonly NpgsqlConnection _connection;

    public MaterialRepository(IConfiguration configuration) : base(configuration)
    {
        _connection = new NpgsqlConnection(ConnectionString);
    }
    public async Task<Material> GetById(int id, CancellationToken ct)
    {
        var cmd = GetByIdCommand(id, ct);

        return await _connection.QueryFirstOrDefaultAsync<Material>(cmd);
    }

    public async Task<IEnumerable<Material>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllCommand(ct);

        return await _connection.QueryAsync<Material>(cmd);
    }

    public async Task<Material> Insert(Material material, CancellationToken ct)
    {
        var cmd = InsertCommand(material, ct);

        var id = await _connection.ExecuteScalarAsync<int>(cmd);

        return await GetById(id, ct);
    }

    public async Task<Material> Update(Material material, CancellationToken ct)
    {
        var cmd = UpdateCommand(material, ct);

        var id = await _connection.ExecuteScalarAsync<int>(cmd);

        return await GetById(id, ct);
    }

    public async Task Delete(int id, CancellationToken ct)
    {
        var cmd = DeleteCommand(id, ct);

        await _connection.ExecuteAsync(cmd);
    }
}
