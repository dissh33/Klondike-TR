using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.Contracts;
public interface ICollectionItemRepository : IGenericRepository<CollectionItem>
{
    Task<CollectionItem> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<CollectionItem>> GetAll(CancellationToken ct);
    Task<CollectionItem> Insert(CollectionItem collectionItem, CancellationToken ct);
    Task<CollectionItem> Update(CollectionItem collectionItem, CancellationToken ct);
    Task<CollectionItem> UpdateName(Guid id, string? name, CancellationToken ct);
    Task<CollectionItem> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct);
    Task<CollectionItem> UpdateCollection(Guid id, Guid? collectionId, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}
