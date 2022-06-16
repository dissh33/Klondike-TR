namespace Offers.Domain.Entities;

public class OfferItem : BaseEntity
{
    public Guid OfferPositionId { get; set; }
    public Guid ItemId { get; set; }
    public int Type { get; set; }
    public int Amount { get; set; } 
}
