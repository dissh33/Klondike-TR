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
    [Fact]
    public void Collection_ShouldConstruct_WithVariousParameters()
    {
        var id = Guid.NewGuid();
        var collection1 = new Collection("N1");
        var collection2 = new Collection("N2", id: id);
        var collection3 = new Collection("N3", status: ItemStatus.Active, externalId: "newID");

        collection1.Should().BeOfType(typeof(Collection));
        collection2.Should().BeOfType(typeof(Collection));
        collection3.Should().BeOfType(typeof(Collection));
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
    public void Collection_ShouldConstruct_WithSpecifiedId_WhenIdPassedThroughConstructor()
    {
        var id = Guid.NewGuid();
        var collection = new Collection("N", id: id);
        
        collection.Id.Should().Be(id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithActiveItemStatus_WhenStatusNotSpecified()
    {
        var collection = new Collection("N");

        collection.Status.Should().HaveFlag(ItemStatus.Active);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedItems_WhenItemsPassedThroughConstructor()
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
    public void Collection_ShouldConstruct_WithSpecifiedItemStatus_WhenStatusPassedThroughConstructor()
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

        for (int i = 0; i < 4; i++)
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

        for (int i = 0; i < 6; i++)
        {
            var item = new CollectionItem("N", collectionId);
            items.Add(item);
        }

        var act = () => collection.Fill(items);

        act.Should().Throw<WrongCollectionItemsNumberException>();
    }
}