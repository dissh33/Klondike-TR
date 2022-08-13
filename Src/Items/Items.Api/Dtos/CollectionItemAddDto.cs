namespace Items.Api.Dtos;

public record CollectionItemAddDto
{
    public string? Name { get; init; }
    public IconAddDto Icon { get; init; } = new();
}
