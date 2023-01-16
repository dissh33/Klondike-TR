using Offers.Domain.Entities;

namespace Offers.Application.Contracts;

public interface IOfferRepository : IBaseGenericRepository<Offer>
{
    Task<Offer?> GetById(Guid id, CancellationToken ct);
    Task<Offer?> Insert(Offer offer, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}