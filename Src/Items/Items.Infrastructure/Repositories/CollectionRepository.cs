using System.Data;
using App.Metrics;
using Dapper;
using Items.Api.Queries.Collection;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Infrastructure.Logging;
using Serilog;

namespace Items.Infrastructure.Repositories;

public class CollectionRepository : BaseRepository<Collection>, ICollectionRepository
{
    public CollectionRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics)
        : base(transaction, logger, metrics)
    {

    }

    public async Task<Collection> GetById(Guid id, CancellationToken ct)            
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<Collection>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<Collection>> GetAll(CancellationToken ct)
    {
        var command = GetAllBaseCommand(ct);

        var query = async () => await Connection.QueryAsync<Collection>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<Collection>> GetAllActive(CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var activeStatus = (int) ItemStatus.Active;

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE status='{activeStatus}'",
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Collection>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<Collection>> GetByFilter(CollectionGetByFilterQuery filter, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var whereClause = filter.GenerateSql();

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} {whereClause}",
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Collection>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<Collection> Insert(Collection collection, CancellationToken ct)
    {
        var command = InsertBaseCommand(collection, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<Collection> Update(Collection collectionItem, CancellationToken ct)
    {
        var command = UpdateBaseCommand(collectionItem, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<Collection> UpdateName(Guid id, string? name, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SCHEMA_NAME}.{TableName} SET name=@name WHERE id = @id RETURNING id",
            parameters: new { id, name },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<Collection> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SCHEMA_NAME}.{TableName} SET icon_id=@iconId WHERE id = @id RETURNING id",
            parameters: new { id, iconId },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<Collection> UpdateStatus(Guid id, int status, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SCHEMA_NAME}.{TableName} SET status=@status WHERE id = @id RETURNING id",
            parameters: new { id, status },
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
}
