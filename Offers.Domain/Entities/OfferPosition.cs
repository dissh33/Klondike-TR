using Offers.Domain.Enums;
using Offers.Domain.SeedWork;
using Offers.Domain.TypedIds;

namespace Offers.Domain.Entities;

public class OfferPosition : BaseEntity
{
    public OfferPositionId? Id { get; set; }

    public OfferId? OfferId { get; set; }
    public string? PriceRate { get; set; }
    public bool WithTrader { get; set; }
    public string? Message { get; set; }

    public OfferPositionType Type { get; set; }

    private OfferPosition()
    {
        
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
}
