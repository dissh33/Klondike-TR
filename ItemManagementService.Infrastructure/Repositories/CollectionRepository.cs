using System.Data;
using Dapper;
using ItemManagementService.Api.Queries.Collection;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using ItemManagementService.Domain.Enums;
using ItemManagementService.Infrastructure.Logging;
using Npgsql;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionBaseRepository : BaseRepository<Collection>, ICollectionRepository
{
    public CollectionBaseRepository(IDbTransaction transaction, ILogger logger)
        : base(transaction, logger)
    {

    }

    public async Task<Collection> GetById(Guid id, CancellationToken ct)            
    {
        var cmd = GetByIdBaseCommand(id, ct);

        return await Connection.QueryFirstOrDefaultAsync<Collection>(cmd);
    }

    public async Task<IEnumerable<Collection>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        var query = async () => await Connection.QueryAsync<Collection>(cmd);

        return await Logger.DbCall(query, cmd);
    }

    public async Task<IEnumerable<Collection>> GetByFilter(CollectionGetByFilterQuery filter, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var whereClause = filter.GenerateSql();

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SchemaName}.{TableName} {whereClause}",
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Collection>(command);

        return await Logger.DbCall(query, command);
    }

    public async Task<Collection> Insert(Collection collectionItem, CancellationToken ct)
    {
        var cmd = InsertBaseCommand(collectionItem, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(cmd);

        var id = await Logger.DbCall(query, cmd);

        return await GetById(id, ct);
    }

    public async Task<Collection> Update(Collection collectionItem, CancellationToken ct)
    {
        var cmd = UpdateBaseCommand(collectionItem, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(cmd);

        var id = await Logger.DbCall(query, cmd);

        return await GetById(id, ct);
    }

    public async Task<Collection> UpdateName(Guid id, string? name, CancellationToken ct)           //TODO: Add Updates Methods
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET name=@name WHERE id = @id RETURNING id",
            parameters: new { id, title = name },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Collection> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET icon_id=@iconId WHERE id = @id RETURNING id",
            parameters: new { id, iconId },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Collection> UpdateStatus(Guid id, int status, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET status=@status WHERE id = @id RETURNING id",
            parameters: new { id, status },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var cmd = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(cmd);

        return await Logger.DbCall(query, cmd);
    }

    override public void Dispose()
    {
        Connection?.Close();
        Connection?.Dispose();
        base.Dispose();
    }
}
