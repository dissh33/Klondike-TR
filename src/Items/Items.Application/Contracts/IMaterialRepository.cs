﻿
using Items.Api.Queries.Material;
using Items.Domain.Entities;

namespace Items.Application.Contracts;
public interface IMaterialRepository : IBaseGenericRepository<Material>
{
    Task<Material?> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<Material>> GetAll(CancellationToken ct);
    Task<IEnumerable<Material>> GetAllAvailable(CancellationToken ct);
    Task<IEnumerable<Material>> GetByFilter(MaterialGetByFilterQuery filter, CancellationToken ct);
    Task<Material?> Insert(Material material, CancellationToken ct);
    Task<Material?> Update(Material material, CancellationToken ct);
    Task<Material?> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct);
    Task<Material?> UpdateStatus(Guid id, int status, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}
