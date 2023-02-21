using Offers.Domain.Entities;

namespace Offers.Application.Contracts;

public interface IOfferRepository : IBaseGenericRepository<Offer>
{
    Task<long> GetCount(CancellationToken ct);

    Task<IEnumerable<Offer>?> GetByPage(
        int page, 
        int pageSize, 
        CancellationToken ct,
        Dictionary<string, string?>? orderByRequest = null);

    Task<Offer?> GetById(Guid id, CancellationToken ct);
    Task<Offer?> Insert(Offer offer, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}