namespace Items.Api.Dtos;

public record MaterialDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public int? Type { get; init; }
    public Guid? IconId { get; init; }
    public int? Status { get; init; }
}