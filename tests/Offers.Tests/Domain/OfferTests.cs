using FluentAssertions;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Xunit;

namespace Offers.Tests.Domain;

public class OfferTests
{
    [Theory]
    [MemberData(nameof(OfferConstructorParameters))]
    public void Offer_ShouldConstruct_WithVariousParameters(
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
}