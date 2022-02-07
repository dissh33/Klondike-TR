using ItemManagementService.Api.Queries.Collection;
using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.Contracts;
public interface ICollectionRepository : IGenericRepository<Collection>
{
    Task<Collection> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<Collection>> GetAll(CancellationToken ct);
    Task<IEnumerable<Collection>> GetByFilter(CollectionGetByFilterQuery filter, CancellationToken ct);
    Task<Collection> Insert(Collection collectionItem, CancellationToken ct);
    Task<Collection> Update(Collection collectionItem, CancellationToken ct);
    Task<Collection> UpdateName(Guid id, string? name, CancellationToken ct);
    Task<Collection> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct);
    Task<Collection> UpdateStatus(Guid id, int status, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}
