using FluentAssertions;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.TypedIds;
using Xunit;

namespace Offers.Tests.Domain;

public class OfferItemTests
{
    [Theory]
    [MemberData(nameof(OfferItemConstructorParameters))]
    public void OfferItem_ShouldConstruct_WithVariousParameters(
        OfferPositionId? offerPositionId,
        Guid tradableItemId,
        int amount,
        OfferItemType type,
        Guid? id)
    {
        var offerItem = new OfferItem(offerPositionId, tradableItemId, amount, type, id);

        offerItem.Should().BeOfType(typeof(OfferItem));
    }

    private static IEnumerable<object?[]> OfferItemConstructorParameters()
    {
        return new List<object?[]>
        {
            new object?[] { new OfferPositionId(), Guid.Empty, 1, default, null },
            new object?[] { new OfferPositionId(), Guid.Empty, 1, OfferItemType.Sell, Guid.Empty },
            new object?[] { null, Guid.Empty, 1, default, null },
        };
    }

    public void OfferItem_ShouldConstruct_EveryTimeWithNewId_WhenIdNotSpecified()
    {

    }
}