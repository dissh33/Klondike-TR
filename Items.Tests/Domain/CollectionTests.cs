using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Xunit;

namespace Items.Tests.Domain;

public class CollectionTests
{
    [Fact]
    public void Collection_ShouldConstruct_WithVariousParameters()
    {
        var id = Guid.NewGuid();
        var collection1 = new Collection("N1", Guid.Empty, new List<CollectionItem>());
        var collection2 = new Collection("N2", Guid.Empty, new List<CollectionItem>(), id: id);
        var collection3 = new Collection("N3", Guid.Empty, new List<CollectionItem>(), externalId: "newID");
        var collection4 = new Collection();

        collection1.Should().BeOfType(typeof(Collection));
        collection2.Should().BeOfType(typeof(Collection));
        collection3.Should().BeOfType(typeof(Collection));
        collection4.Should().BeOfType(typeof(Collection));
    }

    [Fact]
    public void Collection_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var collection1 = new Collection("N", Guid.Empty, new List<CollectionItem>());
        var collection2 = new Collection("N", Guid.Empty, new List<CollectionItem>());

        collection1.Id.Should().NotBeEmpty();
        collection2.Id.Should().NotBeEmpty();

        collection1.Id.Should().NotBe(collection2.Id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedId_WhenIdPassedThroughConstructor()
    {
        var id = Guid.NewGuid();
        var collection = new Collection("N", Guid.Empty, new List<CollectionItem>(), id: id);

        collection.Id.Should().NotBeEmpty();
        collection.Id.Should().Be(id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithActiveItemStatus_WhenStatusNotSpecified()
    {
        var collection = new Collection("N", Guid.Empty, new List<CollectionItem>());

        collection.Status.Should().HaveFlag(ItemStatus.Active);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedItemStatus_WhenStatusPassedThroughConstructor()
    {
        var status = ItemStatus.Disabled;
        var collection = new Collection("N", Guid.Empty, new List<CollectionItem>(), status: status);

        collection.Status.Should().HaveFlag(status);
    }
}