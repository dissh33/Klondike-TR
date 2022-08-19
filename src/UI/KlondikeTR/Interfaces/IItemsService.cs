using Items.Api.Dtos.CollectionItem;
using Offers.Api.Dtos;

namespace KlondikeTR.Interfaces;

public interface IItemsService
{
    Task<IEnumerable<TradableItemDto>> GetAllAvailableItems(CancellationToken ct);
    Task<IEnumerable<CollectionItemDto>> GetCollectionItemsItems(Guid collectionId, CancellationToken ct);
}