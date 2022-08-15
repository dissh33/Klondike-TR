using Items.Api.Dtos.Icon;

namespace Items.Api.Dtos.CollectionItem;

public record CollectionItemFullDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public Guid? CollectionId { get; init; }
    public IconDto? Icon { get; init; }
}
