using Items.Domain.Enums;
using Items.Domain.Exceptions;

namespace Items.Domain.Entities;

public class Collection : BaseEntity, ITradableItem
{
    public string? Name { get; }

    private readonly List<CollectionItem> _items = new();
    public IReadOnlyList<CollectionItem> Items => _items;

    public Icon? Icon { get; private set; }
    public Guid IconId => Icon?.Id ?? Guid.Empty;

    public ItemStatus? Status { get; }
    public DateTime DateAdded { get; }
    

    public Collection()
    {
        DateAdded = DateTime.UtcNow;
    }

    public Collection(
        string? name, 
        List<CollectionItem> items,
        ItemStatus status = ItemStatus.Active, 
        Guid? id = null, 
        string? externalId = null)
        : base (id, externalId)
    {
        Name = name;
        Status = status;

        DateAdded = DateTime.UtcNow;

        Fill(items);
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

    private void Fill(List<CollectionItem> items)
    {
        if (items.Count != 5) throw new WrongNumberOfCollectionItemsException(items.Count);

        _items.AddRange(items);
    }
}