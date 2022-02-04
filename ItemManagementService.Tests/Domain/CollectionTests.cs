using System;
using FluentAssertions;
using ItemManagementService.Domain.Entities;
using ItemManagementService.Domain.Enums;
using Xunit;

namespace ItemManagementService.Tests.Domain;

public class CollectionTests
{
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

        collection.Id.Should().NotBeEmpty();
        collection.Id.Should().Be(id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithActiveItemStatus_WhenStatusNotSpecified()
    {
        var collection = new Collection("N");

        collection.Status.Should().HaveFlag(ItemStatus.Active);
    }
}