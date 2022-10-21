﻿using Offers.Domain.Entities;

namespace Offers.Application.Contracts;

public interface IOfferItemRepository : IBaseGenericRepository<OfferItem>
{
    Task<OfferItem?> GetById(Guid id, CancellationToken ct);
    Task<IEnumerable<OfferItem>> GetByPosition(Guid positionId, CancellationToken ct);
    Task<OfferItem?> Insert(OfferItem offerItem, CancellationToken ct);
    Task<IEnumerable<OfferItem>> BulkInsert(IEnumerable<OfferItem> offerPositions, CancellationToken ct);
    Task<int> Delete(Guid id, CancellationToken ct);
}