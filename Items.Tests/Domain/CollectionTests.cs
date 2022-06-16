using System;
using FluentAssertions;
using ItemManagementService.Domain.Entities;
using ItemManagementService.Domain.Enums;
using Xunit;

namespace ItemManagementService.Tests.Domain;

public class CollectionTests
{
    [Fact]
    public void Collection_ShouldConstruct_WithVariousParameters()
    {
        var id = Guid.NewGuid();
        var collection1 = new Collection("N1", Guid.Empty);
        var collection2 = new Collection("N2", Guid.Empty, id: id);
        var collection3 = new Collection("N3", Guid.Empty, externalId: "newID");
        var collection4 = new Collection();

        collection1.Should().BeOfType(typeof(Collection));
        collection2.Should().BeOfType(typeof(Collection));
        collection3.Should().BeOfType(typeof(Collection));
        collection4.Should().BeOfType(typeof(Collection));
    }

    [Fact]
    public void Collection_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var collection1 = new Collection("N", Guid.Empty);
        var collection2 = new Collection("N", Guid.Empty);

        collection1.Id.Should().NotBeEmpty();
        collection2.Id.Should().NotBeEmpty();

        collection1.Id.Should().NotBe(collection2.Id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedId_WhenIdPassedThroughConstructor()
    {
        var id = Guid.NewGuid();
        var collection = new Collection("N", Guid.Empty, id: id);

        collection.Id.Should().NotBeEmpty();
        collection.Id.Should().Be(id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithActiveItemStatus_WhenStatusNotSpecified()
    {
        var collection = new Collection("N", Guid.Empty);

        collection.Status.Should().HaveFlag(ItemStatus.Active);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedItemStatus_WhenStatusPassedThroughConstructor()
    {
        var status = ItemStatus.Disabled;
        var collection = new Collection("N", Guid.Empty, status: status);

        collection.Status.Should().HaveFlag(status);
    }
}