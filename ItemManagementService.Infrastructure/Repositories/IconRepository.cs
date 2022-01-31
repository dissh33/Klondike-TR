using System.Diagnostics;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using ItemManagementService.Infrastructure.Logging;
using Newtonsoft.Json;
using Npgsql;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class IconRepository : Repository<Icon>, IIconRepository
{
    private readonly ILogger _logger;
    private readonly NpgsqlConnection _connection;

    public IconRepository(string connectionString, ILogger logger)
    {
        _logger = logger;
        _connection = new NpgsqlConnection(connectionString);
    }

    public async Task<Icon> GetById(Guid id, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Where(col => col != "FileBinary").Select(InsertUnderscoreBeforeUpperCase));
        
        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SchemaName}.{TableName} WHERE id = @id",
            parameters: new { id },
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        var query = async () => await _connection.QueryFirstOrDefaultAsync<Icon>(command);

        return await _logger.DbCall(query, command);
    }

    public async Task<Icon> GetFile(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await _connection.QueryFirstOrDefaultAsync<Icon>(command);

        return await _logger.DbCall(query, command);
    }

    public async Task<IEnumerable<Icon>> GetAll(CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Where(col => col != "FileBinary").Select(InsertUnderscoreBeforeUpperCase));
        
        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SchemaName}.{TableName}",
            parameters: new { },
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        var query = async () => await _connection.QueryAsync<Icon>(command);

        return await _logger.DbCall(query, command);
    }

    public async Task<Icon> Insert(Icon icon, CancellationToken ct)
    {
        var command = InsertBaseCommand(icon, ct);

        var query = async () => await _connection.ExecuteScalarAsync<Guid>(command);
        
        var id = await _logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Icon> Update(Icon icon, CancellationToken ct)
    {
        var command = UpdateBaseCommand(icon, ct);

        var query = async () => await _connection.ExecuteScalarAsync<Guid>(command);

        var id = await _logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        var query = async () => await _connection.ExecuteAsync(command);

        return await _logger.DbCall(query, command);
    }

    override public void Dispose()
    {
        _connection.Close();
        _connection.DisposeAsync();
        base.Dispose();
    }
}
