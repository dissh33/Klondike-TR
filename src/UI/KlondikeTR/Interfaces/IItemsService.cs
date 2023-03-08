using Items.Api.Dtos;
using Items.Api.Dtos.CollectionItem;
using Offers.Api.Dtos;

namespace KlondikeTR.Interfaces;

public interface IItemsService
{
    Task<GroupedTradableItemsDto> GetAllAvailableItems(CancellationToken ct);
    Task<string> GetAllAvailableItemsTest(CancellationToken ct);
    Task<IEnumerable<CollectionItemDto>> GetCollectionItems(Guid collectionId, CancellationToken ct);
}