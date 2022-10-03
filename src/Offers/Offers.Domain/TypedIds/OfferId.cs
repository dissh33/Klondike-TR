using Offers.Domain.SeedWork;

namespace Offers.Domain.TypedIds;

public class OfferId : TypedIdValue
{
    public OfferId()
    {
    }

    public OfferId(Guid? value) : base(value)
    {
    }
}
