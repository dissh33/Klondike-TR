namespace Items.Api.Dtos;

public record MaterialFullDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public int? Type { get; init; }
    public int? Status { get; init; }
    public IconDto? Icon { get; init; }
}
