using Offers.Domain.Enums;
using Offers.Domain.SeedWork;
using Offers.Domain.TypedIds;

namespace Offers.Domain.Entities;

public class OfferItem : BaseEntity
{
    public OfferItemId? Id { get; set; }

    public OfferPositionId? OfferPositionId { get; set; }
    public Guid ItemId { get; set; }
    public int Amount { get; set; }

    public OfferItemType Type { get; set; }

    private OfferItem()
    {
        
    }

    public OfferItem(
        OfferPositionId offerPositionId,
        Guid itemId,
        int amount,
        OfferItemType type,
        Guid? id = null)
    {
        Id = new OfferItemId(id);
        OfferPositionId = offerPositionId;
        ItemId = itemId;
        Amount = amount;
        Type = type;
    }
}
