using Offers.Domain.Enums;
using Offers.Domain.SeedWork;
using Offers.Domain.TypedIds;

namespace Offers.Domain.Entities;

public class OfferPosition : BaseEntity
{
    public OfferPositionId Id { get; }

    public OfferId? OfferId { get; }
    public string? PriceRate { get; }
    public bool WithTrader { get; }
    public string? Message { get; }

    public OfferPositionType Type { get; }

    private readonly List<OfferItem> _offerItems = new();
    public IReadOnlyList<OfferItem> OfferItems => _offerItems;

    private OfferPosition()
    {
        Id = new OfferPositionId();
    }

    public OfferPosition(
        OfferId? offerId,
        string? priceRate,
        bool withTrader,
        string? message,
        OfferPositionType type,
        Guid? id = null)
    {
        Id = new OfferPositionId(id);
        OfferId = offerId;
        PriceRate = priceRate;
        WithTrader = withTrader;
        Message = message;
        Type = type;
    }

    public void AddOfferItem(
        Guid tradableItemId,
        int amount,
        OfferItemType type,
        Guid? id = null)
    {
        var offerItem = new OfferItem(Id, tradableItemId, amount, type, id);
        _offerItems.Add(offerItem);
    }
}
