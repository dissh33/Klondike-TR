using Offers.Domain.Entities;

namespace Offers.Application.Contracts;

public interface IOfferItemRepository : IBaseGenericRepository<OfferItem>
{
    Task<OfferItem?> GetById(Guid id, CancellationToken ct);
    Task<OfferItem?> Insert(OfferItem offerItem, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}