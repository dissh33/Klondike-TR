using Items.Domain.Enums;

namespace Items.Domain.Entities;

public class Material : BaseEntity, ITradableItem
{
    public Guid IconId { get; }
    public string? Name { get; }
    public MaterialType Type { get; }
    public ItemStatus Status { get; }
    public DateTime DateAdded { get; }

    public Material()
    {
        DateAdded = DateTime.UtcNow;
    }

    public Material(
        string? name, 
        Guid iconId, 
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