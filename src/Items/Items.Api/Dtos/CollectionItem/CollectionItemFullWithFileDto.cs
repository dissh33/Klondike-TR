using Items.Api.Dtos.Icon;

namespace Items.Api.Dtos.CollectionItem;

public record CollectionItemFullWithFileDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public Guid? CollectionId { get; init; }
    public IconFileDto? Icon { get; init; }
}