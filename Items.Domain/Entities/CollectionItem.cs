namespace Items.Domain.Entities;

public class CollectionItem : BaseEntity, ITradableItem
{
    public string? Name { get; }
    public Guid? CollectionId { get; }

    public Icon? Icon { get; private set; }
    public Guid IconId => Icon?.Id ?? Guid.Empty;

    public CollectionItem()
    {

    }

    public CollectionItem(
        string? name, 
        Guid collectionId, 
        Guid? id = null, 
        string? externalId = null)
        : base(id, externalId)
    {
        Name = name;
        CollectionId = collectionId;
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
    }
}