namespace Offers.Api.Dtos;

public record OfferItemDto
{
    public Guid Id { get; init; }

    public int Amount { get; init; }
    public int Type { get; init; }

    public Guid? OfferPositionId { get; init; }
    public Guid TradableItemId { get; set; }
}