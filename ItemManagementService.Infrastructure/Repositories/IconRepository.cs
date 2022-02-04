using System.Data;
using System.Diagnostics;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using ItemManagementService.Infrastructure.Logging;
using Newtonsoft.Json;
using Npgsql;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class IconBaseRepository : BaseRepository<Icon>, IIconRepository
{
    public IconBaseRepository(IDbTransaction transaction, ILogger logger)
    : base(transaction, logger)
    {

    }

    public async Task<Icon> GetById(Guid id, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Where(col => col != "FileBinary").Select(InsertUnderscoreBeforeUpperCase));
        
        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SchemaName}.{TableName} WHERE id = @id",
            parameters: new { id },
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<Icon>(command);

        return await Logger.DbCall(query, command);
    }

    public async Task<Icon> GetFile(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<Icon>(command);

        return await Logger.DbCall(query, command);
    }

    public async Task<IEnumerable<Icon>> GetAll(CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Where(col => col != "FileBinary").Select(InsertUnderscoreBeforeUpperCase));
        
        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SchemaName}.{TableName}",
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Icon>(command);

        return await Logger.DbCall(query, command);
    }

    public async Task<Icon> Insert(Icon icon, CancellationToken ct)
    {
        var command = InsertBaseCommand(icon, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);
        
        var id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Icon> Update(Icon icon, CancellationToken ct)
    {
        var command = UpdateBaseCommand(icon, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Icon> UpdateTitle(Guid id, string? title, CancellationToken ct)        
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET title=@title WHERE id = @id RETURNING id",
            parameters: new {id, title},
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Icon> UpdateFile(Guid id, string fileName, byte[] fileBinary, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET file_name=@fileName, file_binary=@fileBinary WHERE id = @id RETURNING id",
            parameters: new {id, fileName, fileBinary},
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(command);

        return await Logger.DbCall(query, command);
    }

    override public void Dispose()
    {
        Connection?.Close();
        Connection?.Dispose();
        base.Dispose();
    }
}
