namespace Offers.Api.Dtos;

public class OfferPositionDto
{
    public Guid? OfferId { get; init; }
    public string? PriceRate { get; init; }
    public bool WithTrader { get; init; }
    public string? Message { get; init; }

    public int Type { get; init; }

    public IEnumerable<OfferItemDto> OfferItems { get; init; } = Enumerable.Empty<OfferItemDto>();
}