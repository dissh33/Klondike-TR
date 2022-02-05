namespace ItemManagementService.Domain.Entities;

public class CollectionItem : BaseEntity, ITradableItem
{
    public string? Name { get; }
    public Guid? CollectionId { get; }
    public Guid? IconId { get; }

    public CollectionItem()
    {
        
    }

    public CollectionItem(string? name, Guid? collectionId, Guid? iconId = null, Guid? id = null, string? externalId = null)
        : base(id, externalId)
    {
        Name = name;
        CollectionId = collectionId;
        IconId = iconId;
    }
}