namespace Offers.Api.Dtos;

public record OfferPositionDto
{
    public Guid Id { get; init; }

    public Guid? OfferId { get; init; }
    public string? PriceRate { get; init; }
    public bool WithTrader { get; init; }
    public string? Message { get; init; }

    public int Type { get; init; }

    public IEnumerable<OfferItemDto> OfferItems { get; set; } = Enumerable.Empty<OfferItemDto>();
}