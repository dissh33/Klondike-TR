namespace Offers.Domain.Exceptions;

public class MissingOfferItemsException : DomainException
{
    public MissingOfferItemsException() 
        : base("Offer position must contains at least one item.")
    {
    }
}
