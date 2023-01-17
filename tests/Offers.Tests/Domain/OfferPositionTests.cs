using FluentAssertions;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.Exceptions;
using Offers.Domain.TypedIds;
using Xunit;

namespace Offers.Tests.Domain;

public class OfferPositionTests
{
    [Theory]
    [MemberData(nameof(OfferPositionConstructorParameters))]
    internal void OfferPosition_ShouldConstruct_WithVariousParameters(
        OfferId? offerId,
        string? priceRate,
        bool withTrader,
        string? message,
        OfferPositionType type,
        Guid? id)
    {
        var offerPosition = new OfferPosition(offerId, priceRate, withTrader, message, type, id);

        offerPosition.Should().BeOfType(typeof(OfferPosition));
    }

    private static IEnumerable<object?[]> OfferPositionConstructorParameters()
    {
        return new List<object?[]>
        {
            new object?[] { new OfferId(), "1to1", true, "msg", OfferPositionType.Buying, Guid.Empty },
            new object?[] { null, null, default, null, default, null },
            new object?[] { new OfferId(), "1to1", false, null, default, Guid.Empty },
            new object?[] { new OfferId(), null, false, null, OfferPositionType.ByCollectionItems, null },
        };
    }

    [Fact]
    internal void OfferPosition_ShouldConstruct_WithProvidedOfferItemsList()
    {
        var offerPositionId = new OfferPositionId(Guid.NewGuid());

        var offerItems = new List<OfferItem>
        {
            new(offerPositionId, Guid.Empty, 101, OfferItemType.Buy),
            new(offerPositionId, Guid.Empty, 102, OfferItemType.Buy),
            new(offerPositionId, Guid.Empty, 103, OfferItemType.Buy),
        };

        var position = new OfferPosition(
            new OfferId(), 
            "1to1", 
            true, 
            "msg", 
            default, 
            offerPositionId.Value, 
            offerItems);
        
        position.Should().BeOfType(typeof(OfferPosition));

        position.OfferItems.Should().HaveCount(3);
        position.OfferItems.Select(item => item.OfferPositionId).Should().AllBeEquivalentTo(position.Id);
    }

    [Fact]
    internal void OfferPosition_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var position1 = new OfferPosition(new OfferId(), "1to1", true, "msg", default);
        var position2 = new OfferPosition(new OfferId(), "1to1", true, "msg", default);

        position1.Id.Should().NotBe(position2.Id);
    }

    [Fact]
    internal void OfferPosition_ShouldConstruct_WithSpecifiedId_WhenIdPassed()
    {
        var id = new OfferPositionId(Guid.NewGuid());
        var offerPosition = new OfferPosition(new OfferId(), "1to1", true, "msg", OfferPositionType.Buying, id.Value);

        offerPosition.Id.Should().Be(id);
    }

    [Fact]
    internal void AddOfferItem_ShouldAddOneOfferItemToOfferPosition()
    {
        var offerPosition = new OfferPosition(new OfferId(), "1to1", true, "msg", default);

        offerPosition.AddOfferItem(Guid.Empty, 1, OfferItemType.Buy);

        offerPosition.OfferItems.Should().HaveCount(1);
        offerPosition.OfferItems[0].OfferPositionId.Should().Be(offerPosition.Id);
    }

    [Fact]
    internal void AddOfferItem_ShouldAddOneOfferItem_WithSpecifiedId_WhenOfferItemIdPassed()
    {
        var offerPositionId = new OfferItemId(Guid.NewGuid());
        var offerPosition = new OfferPosition(new OfferId(), "1to1", true, "msg", default);

        offerPosition.AddOfferItem(Guid.Empty, 1, OfferItemType.Buy, offerPositionId.Value);
        
        offerPosition.OfferItems.Should().HaveCount(1);
        offerPosition.OfferItems[0].Id.Should().Be(offerPositionId);
    }

    [Fact]
    internal void AddOfferItems_ShouldAddProvidedOfferItemsList()
    {
        var offerPositionId = new OfferPositionId(Guid.NewGuid());
        var offerPosition = new OfferPosition(new OfferId(), "1to1", true, "msg", default, offerPositionId.Value);

        var offerItems = new List<OfferItem>
        {
            new(null, Guid.Empty, 101, OfferItemType.Buy),
            new(null, Guid.Empty, 102, OfferItemType.Buy),
            new(null, Guid.Empty, 103, OfferItemType.Buy),
        };

        offerPosition.AddOfferItems(offerItems);

        offerPosition.OfferItems.Should().HaveCount(3);
        offerPosition.OfferItems.Select(item => item.OfferPositionId).Should().AllBeEquivalentTo(offerPositionId);
    }

    [Fact]
    internal void AddOfferItems_ShouldThrowException_WhenProvidedEmptyOfferItemsList()
    {
        var offerPosition = new OfferPosition(new OfferId(), "1to1", true, "msg", default);

        var offerItems = new List<OfferItem>();

        var act = () => offerPosition.AddOfferItems(offerItems);

        act.Should().Throw<MissingOfferItemsException>();
    }
}