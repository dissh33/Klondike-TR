using ItemManagementService.Domain.Enums;

namespace ItemManagementService.Domain.Entities;

public class Collection : BaseEntity, ITradableItem
{
    public string? Name { get; set; }
    public Guid? IconId { get; set; }
    public ItemStatus Status { get; set; }
    public DateTime DateAdded { get; set; }

    public List<CollectionItem> CollectionItems { get; set; }

    public Collection()
    {
        DateAdded = DateTime.UtcNow;
        CollectionItems = new List<CollectionItem>();
    }

    public Collection(string name, Guid? iconId = null, ItemStatus status = ItemStatus.Active, Guid? id = null, string? externalId = null)
        :base (id, externalId)
    {
        Name = name;
        IconId = iconId;
        Status = status;
        DateAdded = DateTime.UtcNow;
        CollectionItems = new List<CollectionItem>();
    }
}