using Items.Api.Dtos.CollectionItem;
using KlondikeTR.Interfaces;
using Newtonsoft.Json;
using Offers.Api.Dtos;
using System.Net.Http.Json;

namespace KlondikeTR.Services;

public class ItemsService : IItemsService
{
    private readonly HttpClient _itemsClient;

    public ItemsService(IHttpClientFactory httpClientFactory)
    {
        _itemsClient = httpClientFactory.CreateClient("items");
    }

    public async Task<IEnumerable<TradableItemDto>> GetAllAvailableItems(CancellationToken ct)
    {
        var result = await _itemsClient.GetFromJsonAsync<IEnumerable<TradableItemDto>>("Item", ct);

        return result ?? Enumerable.Empty<TradableItemDto>();
    }

    public async Task<IEnumerable<CollectionItemDto>> GetCollectionItems(Guid collectionId, CancellationToken ct)
    {
        var result = await _itemsClient.GetFromJsonAsync<IEnumerable<CollectionItemDto>>($"CollectionItem/collection/{collectionId}/full", ct);

        return  result ?? Enumerable.Empty<CollectionItemDto>();
    }
}