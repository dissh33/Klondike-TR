using Offers.Domain.SeedWork;

namespace Offers.Domain.TypedIds;

public class OfferPositionId : TypedIdValue
{
    public OfferPositionId()
    {
    }

    public OfferPositionId(Guid? value) : base(value)
    {
    }
}