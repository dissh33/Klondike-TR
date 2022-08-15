using Items.Api.Dtos.Icon;

namespace Items.Api.Dtos.CollectionItem;

public record CollectionItemAddDto
{
    public string? Name { get; init; }
    public IconAddDto Icon { get; init; } = new();
}
