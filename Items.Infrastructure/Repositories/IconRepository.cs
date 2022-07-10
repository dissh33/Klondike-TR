using App.Metrics;
using Dapper;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Infrastructure.Logging;
using Serilog;
using System.Data;

namespace Items.Infrastructure.Repositories;

public class IconRepository : BaseRepository<Icon>, IIconRepository
{
    public IconRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics)
    : base(transaction, logger, metrics)
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

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<Icon> GetFile(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<Icon>(command);

        return await Logger.DbCall(query, command, Metrics);
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

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<Icon>> GetRange(IEnumerable<Guid> ids, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Where(col => col != "FileBinary").Select(InsertUnderscoreBeforeUpperCase));
        var whereClause = string.Join(", ", ids.Select(id => $"'{id}'"));

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SchemaName}.{TableName} WHERE {whereClause}",
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Icon>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<Icon> Insert(Icon icon, CancellationToken ct)
    {
        var command = InsertBaseCommand(icon, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<IEnumerable<Icon>> BulkInsert(IEnumerable<Icon> collectionItems, CancellationToken ct)
    {
        var collectionItemsList = collectionItems.ToList();

        var insertColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var subSql = string.Join(", ", collectionItemsList.Select(x => $"('{x.Title}', '{x.FileBinary}', '{x.FileName}', '{x.Id}')"));

        var sql = $"INSERT INTO {SchemaName}.{TableName} ({insertColumns}) VALUES {subSql};";

        var command = new CommandDefinition(
            commandText: sql,
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteAsync(command);

        await Logger.DbCall(query, command, Metrics);

        return await GetRange(collectionItemsList.Select(icon => icon.Id), ct);
    }

    public async Task<Icon> Update(Icon icon, CancellationToken ct)
    {
        var command = UpdateBaseCommand(icon, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<Icon> UpdateTitle(Guid id, string? title, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET title=@title WHERE id = @id RETURNING id",
            parameters: new { id, title },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<Icon> UpdateFile(Guid id, string fileName, byte[] fileBinary, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET file_name=@fileName, file_binary=@fileBinary WHERE id = @id RETURNING id",
            parameters: new { id, fileName, fileBinary },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    override public void Dispose()
    {
        Connection?.Close();
        Connection?.Dispose();
        base.Dispose();
    }
}
