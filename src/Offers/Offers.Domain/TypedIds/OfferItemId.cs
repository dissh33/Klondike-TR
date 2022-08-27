using Offers.Domain.SeedWork;

namespace Offers.Domain.TypedIds;

public class OfferItemId : TypedIdValue
{
    public OfferItemId(Guid? value) : base(value)
    {
    }
}