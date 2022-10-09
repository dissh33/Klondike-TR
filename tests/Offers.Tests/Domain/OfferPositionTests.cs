using FluentAssertions;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.TypedIds;
using Xunit;

namespace Offers.Tests.Domain;

public class OfferPositionTests
{
    [Theory]
    [MemberData(nameof(OfferPositionConstructorParameters))]
    public void OfferPosition_ShouldConstruct_WithVariousParameters(
        OfferId? offerId,
        string? priceRate,
        bool withTrader,
        string? message,
        OfferPositionType type,
        Guid? id)
    {
        var offerItem = new OfferPosition(offerId, priceRate, withTrader, message, type, id);

        offerItem.Should().BeOfType(typeof(OfferPosition));
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
    public void OfferPosition_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var position1 = new OfferPosition(new OfferId(), "1to1", true, "msg", default);
        var position2 = new OfferPosition(new OfferId(), "1to1", true, "msg", default);

        position1.Id.Should().NotBe(position2.Id);
    }

    [Fact]
    public void OfferPosition_ShouldConstruct_WithSpecifiedId_WhenIdPassed()
    {
        var id = new OfferPositionId(Guid.NewGuid());
        var offerPosition = new OfferPosition(new OfferId(), "1to1", true, "msg", OfferPositionType.Buying, id.Value);

        offerPosition.Id.Should().Be(id);
    }

    [Fact]
    public void AddOfferItem_ShouldAddOneOfferItemToOfferPosition()
    {
        var offerPosition = new OfferPosition(new OfferId(), "1to1", true, "msg", default);

        offerPosition.AddOfferItem(Guid.Empty, 1, OfferItemType.Buy);

        offerPosition.OfferItems.Should().HaveCount(1);
        offerPosition.OfferItems[0].OfferPositionId.Should().Be(offerPosition.Id);
    }

    [Fact]
    public void AddOfferItem_ShouldAddOneOfferItem_WithSpecifiedId_WhenOfferItemIdPassed()
    {
        var offerPosition = new OfferPosition(new OfferId(), "1to1", true, "msg", default);

        var offerPositionId = new OfferItemId(Guid.NewGuid());

        offerPosition.AddOfferItem(Guid.Empty, 1, OfferItemType.Buy, offerPositionId.Value);
        
        offerPosition.OfferItems.Should().HaveCount(1);
        offerPosition.OfferItems[0].Id.Should().Be(offerPositionId);
    }
}