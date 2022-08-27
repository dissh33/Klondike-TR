namespace Offers.Application.Contracts;

public interface IUnitOfWork
{
    IOfferRepository OfferRepository { get; }

    public void Commit();
    public void Rollback();
}