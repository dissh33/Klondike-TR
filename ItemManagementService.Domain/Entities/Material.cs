using ItemManagementService.Domain.Enums;

namespace ItemManagementService.Domain.Entities;

public class Material : BaseEntity, ITradableItem
{
    public Guid? IconId { get; set; }
    public string? Name { get; set; }
    public MaterialType Type { get; set; }
    public ItemStatus Status { get; set; }
    public DateTime DateAdded { get; set; }

    public Material()
    {
        DateAdded = DateTime.UtcNow;
    }

    public Material(
        string? name, 
        Guid? iconId = null, 
        MaterialType type = MaterialType.Default, 
        ItemStatus status = ItemStatus.Active, 
        Guid? id = null, 
        string? externalId = null)
        : base(id, externalId)
    {
        Name = name;
        IconId = iconId;
        Type = type;
        Status = status;

        DateAdded = DateTime.UtcNow;
    }
}