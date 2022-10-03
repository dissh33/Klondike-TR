using System;
using System.Collections.Generic;
using FluentAssertions;
using Items.Domain.Entities;
using Xunit;

namespace Items.Tests.Domain;

public class CollectionItemTests
{
    [Theory]
    [MemberData(nameof(CollectionItemConstructorParameters))]
    public void CollectionItem_ShouldConstruct_WithVariousParameters(
        string? name,
        Guid collectionId,
        Guid? id,
        string? externalId)
    {
        var collectionItem = new CollectionItem(name, collectionId, id, externalId);

        collectionItem.Should().BeOfType(typeof(CollectionItem));
    }

    private static IEnumerable<object?[]> CollectionItemConstructorParameters()
    {
        return new List<object?[]>
        {
            new object?[] { "n1", Guid.Empty, null, null },
            new object?[] { "n2", Guid.Empty, Guid.Empty, null },
            new object?[] { "n3", Guid.Empty, Guid.Empty, "eID" },
        };
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
    public void CollectionItem_ShouldConstruct_WithSpecifiedId_WhenIdPassed()
    {
        var id = Guid.NewGuid();
        var collectionItem = new CollectionItem("N", Guid.Empty, id);
        
        collectionItem.Id.Should().Be(id);
    }

    [Fact]
    public void AddIconMethod_ShouldCorrectlyAddIconToCollectionItem_WhenPassedFullIconInfo()   
    {
        var collectionItem = new CollectionItem("N", Guid.Empty);

        collectionItem.AddIcon("T", new byte[1], "N");
        
        collectionItem.Icon.Should().BeOfType<Icon>();
        collectionItem.IconId.Should().Be(collectionItem.Icon!.Id);
    }

    [Fact]
    public void AddIconMethod_ShouldAddIconWithSpecifiedIdToCollectionItem_WhenPassedOnlyIconId()
    {
        var collectionItem = new CollectionItem("N", Guid.Empty);
        var iconId = Guid.NewGuid();

        collectionItem.AddIcon(iconId);
        
        collectionItem.Icon.Should().BeOfType<Icon>();
        
        collectionItem.IconId.Should().Be(iconId);
        collectionItem.IconId.Should().Be(collectionItem.Icon!.Id);
    }
}