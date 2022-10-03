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
}