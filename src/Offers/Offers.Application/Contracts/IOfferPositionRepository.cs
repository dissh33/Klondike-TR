using Offers.Domain.Entities;

namespace Offers.Application.Contracts;

public interface IOfferPositionRepository : IBaseGenericRepository<OfferPosition>
{
    Task<OfferPosition?> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<OfferPosition>> GetByOffer(Guid offerId, CancellationToken ct);
    Task<OfferPosition?> Insert(OfferPosition offerPosition, CancellationToken ct);
    Task<IEnumerable<OfferPosition>> BulkInsert(IEnumerable<OfferPosition> offerPositions, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}