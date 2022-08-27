using Items.Api.Dtos.CollectionItem;
using KlondikeTR.Interfaces;
using Items.Api.Dtos;

namespace KlondikeTR.Services;

public class ItemsService : IItemsService
{
    private readonly HttpClient _itemsClient;

    public ItemsService(IHttpClientFactory httpClientFactory)
    {
        _itemsClient = httpClientFactory.CreateClient("items");
    }

    public async Task<GroupedTradableItemsDto> GetAllAvailableItems(CancellationToken ct)
    {
        var result = await _itemsClient.GetFromJsonAsync<GroupedTradableItemsDto>("Item", ct);

        return result ?? new GroupedTradableItemsDto();
    }

    public async Task<IEnumerable<CollectionItemDto>> GetCollectionItems(Guid collectionId, CancellationToken ct)
    {
        var result = await _itemsClient.GetFromJsonAsync<IEnumerable<CollectionItemDto>>($"CollectionItem/collection/{collectionId}/full", ct);

        return  result ?? Enumerable.Empty<CollectionItemDto>();
    }
}