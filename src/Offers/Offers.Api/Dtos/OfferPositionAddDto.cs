namespace Offers.Api.Dtos;

public record OfferPositionAddDto
{
    public string? PriceRate { get; init; }
    public bool WithTrader { get; init; }
    public string? Message { get; init; }

    public int Type { get; init; }

    public IEnumerable<OfferItemAddDto> OfferItems { get; init; } = Enumerable.Empty<OfferItemAddDto>();
}