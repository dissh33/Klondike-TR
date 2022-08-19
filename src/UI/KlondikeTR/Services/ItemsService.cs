using KlondikeTR.Interfaces;
using Offers.Api.Dtos;

namespace KlondikeTR.Services;

public class ItemsService : IItemsService
{
    private readonly HttpClient _httpClient;

    public ItemsService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("items");
    }

    //public async Task<IEnumerable<TradableItemDto>> GetAllAvailableItems()
    //{

    //}
}