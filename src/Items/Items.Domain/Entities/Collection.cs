﻿using Items.Domain.Enums;
using Items.Domain.Exceptions;

namespace Items.Domain.Entities;

public class Collection : BaseEntity, ITradableItem
{
    public string? Name { get; }

    private readonly List<CollectionItem> _items = new();
    public IReadOnlyList<CollectionItem> Items => _items;

    public Icon? Icon { get; private set; }
    public Guid IconId { get; private set; }

    public ItemStatus? Status { get; }
    public DateTime DateAdded { get; }
    

    private Collection()
    {
        DateAdded = DateTime.UtcNow;
    }

    public Collection(
        string? name, 
        IEnumerable<CollectionItem>? items = null,
        ItemStatus status = ItemStatus.Available, 
        Guid? id = null, 
        string? externalId = null)
        : base (id, externalId)
    {
        Name = name;
        Status = status;
        
        if (Icon != null) IconId = Icon.Id;

        DateAdded = DateTime.UtcNow;

        if (items is not null)
        {
            Fill(items);
        }
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

    public void AddIcon(Guid id)
    {
        var icon = new Icon(id);

        Icon = icon;
        IconId = Icon.Id;
    }

    public void Fill(IEnumerable<CollectionItem> items)
    {
        var collectionItems = items.ToList();

        if (collectionItems.Count != Constants.COLLECTION_ITEM_NUMBER)
            throw new WrongCollectionItemsNumberException(collectionItems.Count);

        _items.AddRange(collectionItems);
    }
}