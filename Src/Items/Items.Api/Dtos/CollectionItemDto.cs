namespace Items.Api.Dtos;

public record CollectionItemDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public Guid? CollectionId { get; init; }
    public Guid? IconId { get; init; }
}