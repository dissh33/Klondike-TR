namespace Offers.Application.Contracts;

public interface IUnitOfWork
{
    IOfferRepository OfferRepository { get; }
    IOfferPositionRepository OfferPositionRepository { get; }
    IOfferItemRepository OfferItemRepository { get; }

    public void Commit();
    public void Rollback();
}