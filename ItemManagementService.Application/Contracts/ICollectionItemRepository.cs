using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.Contracts;
public interface ICollectionItemRepository : IGenericRepository<CollectionItem>
{
    Task<CollectionItem> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<CollectionItem>> GetAll(CancellationToken ct);
    Task<CollectionItem> Insert(CollectionItem collectionItem, CancellationToken ct);
    Task<CollectionItem> Update(CollectionItem collectionItem, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}
