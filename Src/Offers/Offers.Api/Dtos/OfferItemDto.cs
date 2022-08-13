namespace Offers.Api.Dtos;

public record OfferItemDto
{
    public Guid? OfferPositionId { get; init; }
    public Guid TradableItemId { get; init; }
    public int Amount { get; init; }

    public int Type { get; init; }
}