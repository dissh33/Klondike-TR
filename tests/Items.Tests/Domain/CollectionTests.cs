using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Items.Domain;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Domain.Exceptions;
using Xunit;

namespace Items.Tests.Domain;

public class CollectionTests
{
    [Theory]
    [MemberData(nameof(CollectionConstructorParameters))]
    public void Collection_ShouldConstruct_WithVariousParameters(
        string? name,
        List<CollectionItem>? items,
        ItemStatus status, 
        Guid? id,
        string? externalId)
    {
        var collection = new Collection(name, items, status, id, externalId);

        collection.Should().BeOfType(typeof(Collection));
    }

    private static IEnumerable<object?[]> CollectionConstructorParameters()
    {
        return new List<object?[]>
        {
            new object?[] { "n1", null, default, null, null },
            new object?[] { "n2", null, default, Guid.Empty, null },
            new object?[] { "n3", null, ItemStatus.Disabled, Guid.Empty, "eID" },
        };
    }

    [Fact]
    public void Collection_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var collection1 = new Collection("N");
        var collection2 = new Collection("N");

        collection1.Id.Should().NotBeEmpty();
        collection2.Id.Should().NotBeEmpty();

        collection1.Id.Should().NotBe(collection2.Id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedId_WhenIdPassed()
    {
        var id = Guid.NewGuid();
        var collection = new Collection("N", id: id);
        
        collection.Id.Should().Be(id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithActiveItemStatus_WhenStatusNotSpecified()
    {
        var collection = new Collection("N");

        collection.Status.Should().HaveFlag(ItemStatus.Available);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedItems_WhenItemsPassed()
    {
        var collectionId = Guid.NewGuid();
        var items = new List<CollectionItem>();

        for (int i = 0; i < Constants.COLLECTION_ITEM_NUMBER; i++)
        {
            var item = new CollectionItem("N", collectionId);
            items.Add(item);
        }
        
        var collection = new Collection("N", items: items, id: collectionId);

        collection.Items.Should().BeEquivalentTo(items);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedItemStatus_WhenStatusPassed()
    {
        var status = ItemStatus.Disabled;
        var collection = new Collection("N", status: status);

        collection.Status.Should().HaveFlag(status);
    }

    [Fact]
    public void AddIconMethod_ShouldCorrectlyAddIconToCollection_WhenPassedFullIconInfo()
    {
        var collection = new Collection("N");

        collection.AddIcon("T", new byte[1], "N");
        
        collection.Icon.Should().BeOfType<Icon>();
        collection.IconId.Should().Be(collection.Icon!.Id);
    }

    [Fact]
    public void AddIconMethod_ShouldAddIconWithSpecifiedIdToCollection_WhenPassedOnlyIconId()
    {
        var collection = new Collection("N");
        var iconId = Guid.NewGuid();

        collection.AddIcon(iconId);
        
        collection.Icon.Should().BeOfType<Icon>();
        
        collection.IconId.Should().Be(iconId);
        collection.IconId.Should().Be(collection.Icon!.Id);
    }

    [Fact]
    public void FillMethod_ShouldAddItemsToCollection_WhenPassedCorrectNumberOfItems()
    {
        var collectionId = Guid.NewGuid();

        var collection = new Collection("N", id: collectionId);
        var items = new List<CollectionItem>();

        for (int i = 0; i < Constants.COLLECTION_ITEM_NUMBER; i++)
        {
            var item = new CollectionItem("N", collectionId);
            items.Add(item);
        }

        collection.Fill(items);

        collection.Items.Should().BeEquivalentTo(items);
    }

    [Fact]
    public void FillMethod_ShouldThrowException_WhenPassedLessNumberOfItems()
    {
        var collectionId = Guid.NewGuid();

        var collection = new Collection("N", id: collectionId);
        var items = new List<CollectionItem>();

        for (int i = 0; i < Constants.COLLECTION_ITEM_NUMBER - 1; i++)
        {
            var item = new CollectionItem("N", collectionId);
            items.Add(item);
        }

        var act = () => collection.Fill(items);
        
        act.Should().Throw<WrongCollectionItemsNumberException>();
    }

    [Fact]
    public void FillMethod_ShouldThrowException_WhenPassedMoreNumberOfItems()
    {
        var collectionId = Guid.NewGuid();

        var collection = new Collection("N", id: collectionId);
        var items = new List<CollectionItem>();

        for (int i = 0; i < Constants.COLLECTION_ITEM_NUMBER + 1; i++)
        {
            var item = new CollectionItem("N", collectionId);
            items.Add(item);
        }

        var act = () => collection.Fill(items);

        act.Should().Throw<WrongCollectionItemsNumberException>();
    }
}