﻿using System;
using FluentAssertions;
using Items.Domain.Entities;
using Xunit;

namespace Items.Tests.Domain;

public class CollectionItemTests
{
    [Fact]
    public void CollectionItem_ShouldConstruct_WithVariousParameters()
    {
        var collectionItem1 = new CollectionItem("N1", Guid.Empty);
        var collectionItem2 = new CollectionItem("N2", Guid.Empty, Guid.Empty);
        var collectionItem3 = new CollectionItem("N3", Guid.Empty, Guid.Empty, externalId: "newID");

        collectionItem1.Should().BeOfType(typeof(CollectionItem));
        collectionItem2.Should().BeOfType(typeof(CollectionItem));
        collectionItem3.Should().BeOfType(typeof(CollectionItem));
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
        var collectionItem = new CollectionItem("N", Guid.Empty, id);

        collectionItem.Id.Should().NotBeEmpty();
        collectionItem.Id.Should().Be(id);
    }
}