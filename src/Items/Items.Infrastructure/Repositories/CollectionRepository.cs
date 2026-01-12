using System.Data;
using Dapper;
using Items.Api.Queries.Collection;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Infrastructure.Filters;
using Items.Infrastructure.Metrics;
using Serilog;

namespace Items.Infrastructure.Repositories;

public class CollectionRepository(IDbTransaction transaction, ILogger logger, DbMetrics metrics) 
    : BaseRepository<Collection>(transaction, logger, metrics), ICollectionRepository
{
    public async Task<Collection?> GetById(Guid id, CancellationToken ct)            
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<Collection>(command);

        return await DbCall(query, command);
    }

    public async Task<IEnumerable<Collection>> GetAll(CancellationToken ct)
    {
        var command = GetAllBaseCommand(ct);

        var query = async () => await Connection.QueryAsync<Collection>(command);

        return await DbCall(query, command);
    }

    public async Task<IEnumerable<Collection>> GetAllAvailable(CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        const int activeStatus = (int) ItemStatus.Available;

        var command = CreateCommand(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE status='{activeStatus}'",
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Collection>(command);

        return await DbCall(query, command);
    }

    public async Task<IEnumerable<Collection>> GetByFilter(CollectionGetByFilterQuery request, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var whereClause = (request as CollectionFilter)?.GenerateSql();

        var command = CreateCommand(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} {whereClause}",
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Collection>(command);

        return await DbCall(query, command);
    }

    public async Task<Collection?> Insert(Collection collection, CancellationToken ct)
    {
        var command = InsertBaseCommand(collection, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Collection?> Update(Collection collectionItem, CancellationToken ct)
    {
        var command = UpdateBaseCommand(collectionItem, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Collection?> UpdateName(Guid id, string? name, CancellationToken ct)
    {
        var command = CreateCommand(
            commandText: $"UPDATE {SCHEMA_NAME}.{TableName} SET name=@name WHERE id = @id RETURNING id",
            parameters: new { id, name },
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Collection?> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct)
    {
        var command = CreateCommand(
            commandText: $"UPDATE {SCHEMA_NAME}.{TableName} SET icon_id=@iconId WHERE id = @id RETURNING id",
            parameters: new { id, iconId },
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Collection?> UpdateStatus(Guid id, int status, CancellationToken ct)
    {
        var command = CreateCommand(
            commandText: $"UPDATE {SCHEMA_NAME}.{TableName} SET status=@status WHERE id = @id RETURNING id",
            parameters: new { id, status },
            cancellationToken: ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        id = await DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<int> Delete(Guid id, CancellationToken ct)
    {
        var command = DeleteBaseCommand(id, ct);

        var query = async () => await Connection.ExecuteAsync(command);

        return await DbCall(query, command);
    }
}
