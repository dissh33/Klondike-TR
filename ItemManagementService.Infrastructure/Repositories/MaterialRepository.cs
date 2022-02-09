using System.Data;
using Dapper;
using ItemManagementService.Api.Queries.Material;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using ItemManagementService.Infrastructure.Logging;
using Npgsql;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public class MaterialBaseRepository : BaseRepository<Material>, IMaterialRepository
{
    public MaterialBaseRepository(IDbTransaction transaction, ILogger logger)
        : base(transaction, logger)
    {

    }

    public async Task<Material> GetById(Guid id, CancellationToken ct)
    {
        var command = GetByIdBaseCommand(id, ct);

        var query = async () => await Connection.QueryFirstOrDefaultAsync<Material>(command);

        return await Logger.DbCall(query, command);
    }

    public async Task<IEnumerable<Material>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        var query = async () => await Connection.QueryAsync<Material>(cmd);

        return await Logger.DbCall(query, cmd);
    }

    public async Task<IEnumerable<Collection>> GetByFilter(MaterialGetByFilterQuery filter, CancellationToken ct)
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

    public async Task<Material> Insert(Material material, CancellationToken ct)
    {
        var command = InsertBaseCommand(material, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }

    public async Task<Material> Update(Material material, CancellationToken ct)
    {
        var command = UpdateBaseCommand(material, ct);

        var query = async () => await Connection.ExecuteScalarAsync<Guid>(command);

        var id = await Logger.DbCall(query, command);

        return await GetById(id, ct);
    }
    
    public async Task<Material> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct)
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

    public async Task<Material> UpdateStatus(Guid id, int status, CancellationToken ct)
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
