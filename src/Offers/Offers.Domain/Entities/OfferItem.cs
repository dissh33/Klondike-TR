using Offers.Domain.Enums;
using Offers.Domain.SeedWork;
using Offers.Domain.TypedIds;

namespace Offers.Domain.Entities;

public class OfferItem : BaseEntity
{
    public OfferItemId Id { get; }

    public OfferPositionId? OfferPositionId { get; }
    public Guid TradableItemId { get; }
    
    public int Amount { get; }
    public OfferItemType Type { get; }

    private OfferItem()
    {
        Id = new OfferItemId(Guid.NewGuid());
    }

    public OfferItem(
        OfferPositionId? offerPositionId,
        Guid tradableItemId,
        int amount,
        OfferItemType type,
        Guid? id = null)
    {
        Id = new OfferItemId(id);
        OfferPositionId = offerPositionId;
        TradableItemId = tradableItemId;
        Amount = amount;
        Type = type;
    }
}
