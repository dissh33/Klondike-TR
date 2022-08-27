namespace Items.Api.Dtos.Icon;

public record IconFileDto
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? FileName { get; init; }
    public byte[]? FileBinary { get; init; }
}
