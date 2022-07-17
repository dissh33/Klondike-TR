namespace Offers.Api.Dtos;

public class OfferPositionDto
{
    public Guid? OfferId { get; set; }
    public string? PriceRate { get; set; }
    public bool WithTrader { get; set; }
    public string? Message { get; set; }

    public int? Type { get; set; }

    public IEnumerable<OfferItemDto>? OfferItems { get; set; }
}