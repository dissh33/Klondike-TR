using Items.Api.Dtos.CollectionItem;
using KlondikeTR.Interfaces;
using Newtonsoft.Json;
using Offers.Api.Dtos;

namespace KlondikeTR.Services;

public class ItemsService : IItemsService
{
    private readonly HttpClient _itemsClient;
    private readonly HttpClient _collectionItemsClient;

    public ItemsService(IHttpClientFactory httpClientFactory)
    {
        _itemsClient = httpClientFactory.CreateClient("items");
        _collectionItemsClient = httpClientFactory.CreateClient("collection-items");
    }

    public async Task<IEnumerable<TradableItemDto>> GetAllAvailableItems(CancellationToken ct)
    {
        var httpResponse = await _itemsClient.GetAsync("", ct);
        var stringContent = await httpResponse.Content.ReadAsStringAsync(ct);

        var items = JsonConvert.DeserializeObject<IEnumerable<TradableItemDto>>(stringContent);

        return items ?? Enumerable.Empty<TradableItemDto>();
    }

    public async Task<IEnumerable<CollectionItemDto>> GetCollectionItemsItems(Guid collectionId, CancellationToken ct)
    {
        var httpResponse = await _collectionItemsClient.GetAsync($"/collection/{collectionId}/full", ct);
        var stringContent = await httpResponse.Content.ReadAsStringAsync(ct);

        var items = JsonConvert.DeserializeObject<IEnumerable<CollectionItemDto>>(stringContent);

        return items ?? Enumerable.Empty<CollectionItemDto>();
    }
}