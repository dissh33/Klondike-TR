
using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.Contracts;
public interface ICollectionRepository : IGenericRepository<Collection>
{
    Task<Collection> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<Collection>> GetAll(CancellationToken ct);
    Task<Collection> Insert(Collection collectionItem, CancellationToken ct);
    Task<Collection> Update(Collection collectionItem, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}
