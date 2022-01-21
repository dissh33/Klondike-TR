using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class IconRepository : BaseRepository<Icon>, IIconRepository
{
    private readonly NpgsqlConnection _connection;

    public IconRepository(IConfiguration configuration) : base(configuration)
    {
        _connection = new NpgsqlConnection(ConnectionString);
    }

    public async Task<Icon> GetById(int id, CancellationToken ct)
    {
        var cmd = GetByIdCommand(id, ct);
        
        return await _connection.QueryFirstOrDefaultAsync<Icon>(cmd);
    }

    public async Task<IEnumerable<Icon>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllCommand(ct);

        return await _connection.QueryAsync<Icon>(cmd);
    }

    public async Task<Icon> Insert(Icon icon, CancellationToken ct)
    {
        var cmd = InsertCommand(icon, ct);

        var id = await _connection.ExecuteScalarAsync<int>(cmd);

        return await GetById(id, ct);
    }

    public async Task<Icon> Update(Icon icon, CancellationToken ct)
    {
        var cmd = UpdateCommand(icon, ct);

        var id = await _connection.ExecuteScalarAsync<int>(cmd);

        return await GetById(id, ct);
    }

    public async Task Delete(int id, CancellationToken ct)
    {
        var cmd = DeleteCommand(id, ct);

        await _connection.ExecuteAsync(cmd);
    }
}
