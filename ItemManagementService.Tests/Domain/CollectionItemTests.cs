using System;
using FluentAssertions;
using ItemManagementService.Domain.Entities;
using Xunit;

namespace ItemManagementService.Tests.Domain;

public class CollectionItemTests
{
    [Fact]
    public void CollectionItem_ShouldConstruct_WithVariousParameters()
    {
        var id = Guid.NewGuid();
        var collectionItem1 = new CollectionItem("N1", Guid.Empty, Guid.NewGuid());
        var collectionItem2 = new CollectionItem("N2", Guid.Empty, id: id);
        var collectionItem3 = new CollectionItem("N3", Guid.Empty, externalId: "newID");
        var collectionItem4 = new CollectionItem();

        collectionItem1.Should().BeOfType(typeof(CollectionItem));
        collectionItem2.Should().BeOfType(typeof(CollectionItem));
        collectionItem3.Should().BeOfType(typeof(CollectionItem));
        collectionItem4.Should().BeOfType(typeof(CollectionItem));
    }

    [Fact]
    public void CollectionItem_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var item1 = new CollectionItem("N", Guid.Empty);
        var item2 = new CollectionItem("N", Guid.Empty);

        item1.Id.Should().NotBeEmpty();
        item2.Id.Should().NotBeEmpty();

        item1.Id.Should().NotBe(item2.Id);
    }

    [Fact]
    public void CollectionItem_ShouldConstruct_WithSpecifiedId_WhenIdPassedThroughConstructor()
    {
        var id = Guid.NewGuid();
        var collectionItem = new CollectionItem("N", Guid.Empty, id: id);

        collectionItem.Id.Should().NotBeEmpty();
        collectionItem.Id.Should().Be(id);
    }
}