namespace Offers.Api.Dtos;

public record OfferItemDto
{
    public int Amount { get; init; }
    public int Type { get; init; }

    public Guid? OfferPositionId { get; init; }
    public TradableItemDto? TradableItem { get; init; }
}