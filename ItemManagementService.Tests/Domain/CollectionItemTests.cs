using System;
using FluentAssertions;
using ItemManagementService.Domain.Entities;
using Xunit;

namespace ItemManagementService.Tests.Domain;

public class CollectionItemTests
{
    [Fact]
    public void Collection_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var item1 = new CollectionItem("N");
        var item2 = new CollectionItem("N");

        item1.Id.Should().NotBeEmpty();
        item2.Id.Should().NotBeEmpty();

        item1.Id.Should().NotBe(item2.Id);
    }

    [Fact]
    public void Collection_ShouldConstruct_WithSpecifiedId_WhenIdPassedThroughConstructor()
    {
        var id = Guid.NewGuid();
        var collectionItem = new CollectionItem("N", id: id);

        collectionItem.Id.Should().NotBeEmpty();
        collectionItem.Id.Should().Be(id);
    }
}