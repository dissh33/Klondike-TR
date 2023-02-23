using FluentAssertions;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.TypedIds;
using Xunit;

namespace Offers.Tests.Domain;

public class OfferTests
{
    [Theory]
    [MemberData(nameof(OfferConstructorParameters))]
    internal void Offer_ShouldConstruct_WithVariousParameters(
        string? title,
        string? message,
        string? expression,
        OfferType type,
        OfferStatus status,
        Guid? id)
    {
        var offerItem = new Offer(title, message, expression, type, status, id);

        offerItem.Should().BeOfType(typeof(Offer));
    }

    private static IEnumerable<object?[]> OfferConstructorParameters()
    {
        return new List<object?[]>
        {
            new object?[] { "t1", "msg", "expression", OfferType.New, OfferStatus.Disable, Guid.Empty },
            new object?[] { null, null, null, default, default, Guid.Empty },
            new object?[] { "t1", "msg", "expression", default, default, Guid.Empty },
            new object?[] { null, null, "expression", OfferType.New, OfferStatus.Disable, null },
        };
    }

    [Fact]
    internal void Offer_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var offer1 = new Offer("t1", "msg", "expression");
        var offer2 = new Offer("t2", "msg", "expression");

        offer1.Id.Should().NotBe(offer2.Id);
    }

    [Fact]
    internal void Offer_ShouldConstruct_WithSpecifiedId_WhenIdPassed()
    {
        var id = new OfferId(Guid.NewGuid());
        var offer = new Offer("t1", "msg", "expression", id: id.Value);

        offer.Id.Should().Be(id);
    }

    [Fact]
    internal void AddPosition_ShouldAddOneOfferPositionToOffer_AndReturnAddedOfferPositionId()
    {
        var offer = new Offer("t1", "msg", "expression");

        var offerPositionId = offer.AddPosition("1to1", false, "msg", OfferPositionType.ByCollectionItems);

        offer.Positions.Should().HaveCount(1);
        offer.Positions[0].OfferId.Should().Be(offer.Id);

        offerPositionId.Should().Be(offer.Positions[0].Id);
    }

    [Fact]
    internal void AddPosition_ShouldAddOneOfferPosition_WithSpecifiedId_WhenOfferPositionIdPassed()
    {
        var offer = new Offer("t1", "msg", "expression");

        var offerPositionId = new OfferPositionId(Guid.NewGuid());
        offer.AddPosition("1to1", false, "msg", OfferPositionType.ByCollectionItems, offerPositionId.Value);

        offer.Positions.Should().HaveCount(1);
        offer.Positions[0].Id.Should().Be(offerPositionId);
    }

    [Fact]
    internal void AddOfferPositions_ShouldAddProvidedOfferPositionsList()
    {
        var offerId = new OfferId(Guid.NewGuid());
        var offer = new Offer("t", "msg", "expr", default, default, offerId.Value);

        var offerPositions = new List<OfferPosition>
        {
            new(null, "1to1", true, "m-1", default, offerId.Value),
            new(null, "1to2", true, "m-2", default, offerId.Value),
            new(null, "1to3", true, "m-3", default, offerId.Value),
        };

        offer.AddPositions(offerPositions);

        offer.Positions.Should().HaveCount(3);
        offer.Positions.Select(item => item.OfferId).Should().AllBeEquivalentTo(offerId);
    }
}