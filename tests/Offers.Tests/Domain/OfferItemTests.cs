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

    [Fact]
    public void OfferItem_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var item1 = new OfferItem(new OfferPositionId(), Guid.Empty, 1, default);
        var item2 = new OfferItem(new OfferPositionId(), Guid.Empty, 1, default);
        
        item1.Id.Should().NotBe(item2.Id);
    }

    [Fact]
    public void OfferItem_ShouldConstruct_WithSpecifiedId_WhenIdPassed()
    {
        var id = new OfferItemId(Guid.NewGuid());
        var offerItem = new OfferItem(new OfferPositionId(), Guid.Empty, 1, default, id.Value);

        offerItem.Id.Should().Be(id);
    }
}