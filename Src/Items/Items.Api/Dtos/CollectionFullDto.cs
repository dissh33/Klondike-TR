namespace Items.Api.Dtos;

public record CollectionFullDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public IconDto? Icon { get; init; }
    public int? Status { get; init; }
    public List<CollectionItemFullDto>? Items { get; set; }
}