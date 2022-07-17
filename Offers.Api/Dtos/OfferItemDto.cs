namespace Offers.Api.Dtos;

public class OfferItemDto
{
    public Guid? OfferPositionId { get; set; }
    public Guid TradableItemId { get; set; }
    public int Amount { get; set; }

    public int? Type { get; set; }
}