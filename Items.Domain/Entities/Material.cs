using Items.Domain.Enums;

namespace Items.Domain.Entities;

public class Material : BaseEntity, ITradableItem
{
    public string? Name { get; }

    public Icon? Icon { get; private set; }
    public Guid IconId { get; private set; }

    public MaterialType Type { get; }
    public ItemStatus Status { get; }
    public DateTime DateAdded { get; }

    private Material()
    {
        DateAdded = DateTime.UtcNow;
    }

    public Material(
        string? name, 
        MaterialType type = MaterialType.Default, 
        ItemStatus status = ItemStatus.Active, 
        Guid? id = null, 
        string? externalId = null)
        : base(id, externalId)
    {
        Name = name;
        Type = type;

        if (Icon != null) IconId = Icon.Id;

        Status = status;
        DateAdded = DateTime.UtcNow;
    }

    public void AddIcon(
        string? title,
        byte[]? fileBinary,
        string? fileName,
        Guid? id = null,
        string? externalId = null)
    {
        var icon = new Icon(title, fileBinary, fileName, id, externalId);

        Icon = icon;
        IconId = Icon.Id;
    }
}