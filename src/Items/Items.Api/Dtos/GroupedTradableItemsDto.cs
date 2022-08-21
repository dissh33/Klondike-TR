namespace Items.Api.Dtos;

public record GroupedTradableItemsDto
{
    public IEnumerable<TradableItemDto> Materials { get; init; } = Enumerable.Empty<TradableItemDto>();
    public IEnumerable<TradableItemDto> Collections { get; init; } = Enumerable.Empty<TradableItemDto>();
}