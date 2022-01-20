namespace ItemManagementService.Domain.Entities;

public class CollectionItem : BaseEntity, ITradableItem
{
    public Guid IconId { get; set; }
    public string? Name { get; set; }
    public Collection? Collection { get; set; }
}