﻿using System.Data;
using App.Metrics;
using Dapper;
using Items.Api.Queries.Material;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Infrastructure.Filters;
using Items.Infrastructure.Logging;
using Serilog;

namespace Items.Infrastructure.Repositories;

public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
{
    public MaterialRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics)
        : base(transaction, logger, metrics)
    {

    }

    public async Task<Material?> GetById(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<Material>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<Material>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        var query = async () => await Connection.QueryAsync<Material>(cmd);

        return await Logger.DbCall(query, cmd, Metrics);
    }

    public async Task<IEnumerable<Material>> GetAllAvailable(CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        const int activeStatus = (int) ItemStatus.Available;

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE status='{activeStatus}'",
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Material>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<IEnumerable<Material>> GetByFilter(MaterialGetByFilterQuery request, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var whereClause = (request as MaterialFilter)?.GenerateSql();

        var command = new CommandDefinition(
            commandText: $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} {whereClause}",
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);

        var query = async () => await Connection.QueryAsync<Material>(command);

        return await Logger.DbCall(query, command, Metrics);
    }

    public async Task<Material?> Insert(Material material, CancellationToken ct)
    {
        var command = InsertBaseCommand(material, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }

    public async Task<Material?> Update(Material material, CancellationToken ct)
    {
        var command = UpdateBaseCommand(material, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command, Metrics);

        return await GetById(id, ct);
    }
    
    public async Task<Material?> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct)
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

    public async Task<Material?> UpdateStatus(Guid id, int status, CancellationToken ct)
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
