using System.Data;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
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
        var cmd = GetByIdBaseCommand(id, ct);

        return await Connection.QueryFirstOrDefaultAsync<Material>(cmd);
    }

    public async Task<IEnumerable<Material>> GetAll(CancellationToken ct)
    {
        var cmd = GetAllBaseCommand(ct);

        return await Connection.QueryAsync<Material>(cmd);
    }

    public async Task<Material> Insert(Material material, CancellationToken ct)
    {
        var cmd = InsertBaseCommand(material, ct);

        var id = await Connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task<Material> Update(Material material, CancellationToken ct)
    {
        var cmd = UpdateBaseCommand(material, ct);

        var id = await Connection.ExecuteScalarAsync<Guid>(cmd);

        return await GetById(id, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var cmd = DeleteBaseCommand(id, ct);

        await Connection.ExecuteAsync(cmd);
    }

    override public void Dispose()
    {
        Connection?.Close();
        Connection?.Dispose();
        base.Dispose();
    }
}
