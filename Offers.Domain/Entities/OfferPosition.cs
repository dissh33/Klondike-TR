namespace Offers.Domain.Entities;

public class OfferPosition : BaseEntity
{
    public Guid OfferId { get; set; }
    public int Type { get; set; }
    public string? PriceRate { get; set; }
    public bool WithTrader { get; set; }
    public string? Message { get; set; }
}
