namespace Offers.Api.Dtos;

public record TradableItemDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? ItemType { get; init; }
    public IconFileDto? Icon { get; init; }
    public Guid IconId { get; init; }
}