using Items.Domain.Entities;

namespace Items.Application.Contracts;
public interface ICollectionItemRepository : IBaseGenericRepository<CollectionItem>
{
    Task<CollectionItem> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<CollectionItem>> GetAll(CancellationToken ct);
    Task<IEnumerable<CollectionItem>> GetByCollection(Guid collectionId, CancellationToken ct);
    Task<CollectionItem> Insert(CollectionItem collectionItem, CancellationToken ct);
    Task<IEnumerable<CollectionItem>> BulkInsert(IEnumerable<CollectionItem> collectionItem, CancellationToken ct);
    Task<CollectionItem> Update(CollectionItem collectionItem, CancellationToken ct);
    Task<CollectionItem> UpdateName(Guid id, string? name, CancellationToken ct);
    Task<CollectionItem> UpdateIcon(Guid id, Guid? iconId, CancellationToken ct);
    Task<CollectionItem> UpdateCollection(Guid id, Guid? collectionId, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}
