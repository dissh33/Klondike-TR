namespace ItemManagementService.Domain.Entities;

public class CollectionItem : BaseEntity, ITradableItem
{
    public string? Name { get; set; }
    public Guid? IconId { get; set; }
    public Collection? Collection { get; set; }

    public CollectionItem()
    {
        
    }

    public CollectionItem(string name, Guid? iconId = null, Guid? id = null, string? externalId = null)
        : base(id, externalId)
    {
        Name = name;
        IconId = iconId;
    }
}