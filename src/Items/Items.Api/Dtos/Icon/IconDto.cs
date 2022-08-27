namespace Items.Api.Dtos.Icon;

public record IconDto
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? FileName { get; init; }
}