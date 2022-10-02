using Items.Api.Dtos;
using Items.Api.Dtos.CollectionItem;
using KlondikeTR.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace KlondikeTR.Services;

public class ItemsService : IItemsService
{
    private readonly TimeSpan _itemsCacheAge = TimeSpan.FromMinutes(10);

    private readonly HttpClient _itemsClient;
    private readonly IMemoryCache _memoryCache;

    public ItemsService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _itemsClient = httpClientFactory.CreateClient("items");
    }

    public async Task<GroupedTradableItemsDto> GetAllAvailableItems(CancellationToken ct)
    {
        var result = await _memoryCache.GetOrCreateAsync("all_items", 
            async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = _itemsCacheAge;
                return await _itemsClient.GetFromJsonAsync<GroupedTradableItemsDto>("Item", ct);
            });

        return result ?? new GroupedTradableItemsDto();
    }

    public async Task<IEnumerable<CollectionItemDto>> GetCollectionItems(Guid collectionId, CancellationToken ct)
    {
        var result = await _memoryCache.GetOrCreateAsync("collection_items",
            async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = _itemsCacheAge;
                return await _itemsClient.GetFromJsonAsync<IEnumerable<CollectionItemDto>>($"CollectionItem/collection/{collectionId}/full", ct);
            });

        return  result ?? Enumerable.Empty<CollectionItemDto>();
    }
}