namespace Offers.Api.Dtos;

public record OfferItemAddDto
{
    public int Amount { get; init; }
    public int Type { get; init; }

    public Guid TradableItemId { get; init; }
}