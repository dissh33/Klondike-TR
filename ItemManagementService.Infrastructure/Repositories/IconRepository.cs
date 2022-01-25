using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class IconRepository : Repository<Icon>, IIconRepository
{
    private readonly NpgsqlConnection _connection;

    public IconRepository(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
    }

    public async Task<Icon> GetById(Guid id, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Where(col => col != "FileBinary").Select(InsertUnderscoreBeforeUpperCase));

        var sql = $"SELECT {selectColumns} FROM {SchemaName}.{TableName} WHERE id = @id";

        var command = new CommandDefinition(
            sql,
            new { id },
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        return await _connection.QueryFirstOrDefaultAsync<Icon>(command);
    }

    public async Task<Icon> GetFile(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        return await _connection.QueryFirstOrDefaultAsync<Icon>(command);
    }

    public async Task<IEnumerable<Icon>> GetAll(CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Where(col => col != "FileBinary").Select(InsertUnderscoreBeforeUpperCase));

        var sql = $"SELECT {selectColumns} FROM {SchemaName}.{TableName}";

        var command = new CommandDefinition(
            sql,
            new { },
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        return await _connection.QueryAsync<Icon>(command);
    }

    public async Task<Icon> Insert(Icon icon, CancellationToken ct)
    {
        var command = InsertBaseCommand(icon, ct);

        var id = await _connection.ExecuteScalarAsync<Guid>(command);

        return await GetById(id, ct);
    }

    public async Task<Icon> Update(Icon icon, CancellationToken ct)
    {
        var command = UpdateBaseCommand(icon, ct);

        var id = await _connection.ExecuteScalarAsync<Guid>(command);

        return await GetById(id, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        return await _connection.ExecuteAsync(command);
    }

    override public void Dispose()
    {
        _connection.Close();
        _connection.DisposeAsync();
        base.Dispose();
    }
}
