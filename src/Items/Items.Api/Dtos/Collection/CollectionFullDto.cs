using Items.Api.Dtos.CollectionItem;
using Items.Api.Dtos.Icon;

namespace Items.Api.Dtos.Collection;

public record CollectionFullDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public IconDto? Icon { get; init; }
    public int? Status { get; init; }
    public List<CollectionItemFullDto>? Items { get; set; }
}