using Items.Api.Dtos.Icon;

namespace Items.Api.Dtos;

public record TradableItemDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? ItemType { get; set; }
    public IconFileDto? Icon { get; init; }
    public Guid IconId { get; init; }
}