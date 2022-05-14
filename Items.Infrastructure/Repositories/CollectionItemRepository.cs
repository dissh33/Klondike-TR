﻿using System.Data;
using App.Metrics;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using ItemManagementService.Infrastructure.Logging;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionItemRepository : BaseRepository<CollectionItem>, ICollectionItemRepository
{    public CollectionItemRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics)
        : base(transaction, logger, metrics)
    {

    }

    public async Task<CollectionItem> GetById(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<CollectionItem>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<CollectionItem>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        var query = async () => await Connection.QueryAsync<CollectionItem>(cmd);

        return await Logger.DbCall(query, cmd, Metrics);
    }

    public async Task<IEnumerable<CollectionItem>> GetByCollection(Guid collectionId, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SchemaName}.{TableName} WHERE collection_id = @collectionId",
            parameters: new { collectionId },
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<CollectionItem>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<CollectionItem> Insert(CollectionItem collectionItem, CancellationToken ct)
    {
        var command = InsertBaseCommand(collectionItem, ct);

        var query =  async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<CollectionItem> Update(CollectionItem collectionItem, CancellationToken ct)
    {
        var command = UpdateBaseCommand(collectionItem, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<CollectionItem> UpdateName(Guid id, string? name, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET name=@name WHERE id = @id RETURNING id",
            parameters: new { id, name },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<CollectionItem> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET icon_id=@iconId WHERE id = @id RETURNING id",
            parameters: new { id, iconId },
            transaction: Transaction,
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<CollectionItem> UpdateCollection(Guid id, Guid? collectionId, CancellationToken ct)
    {
        var command = new CommandDefinition(
            commandText: $"UPDATE {SchemaName}.{TableName} SET collection_id=@collectionId WHERE id = @id RETURNING id",
            parameters: new { id, collectionId },
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