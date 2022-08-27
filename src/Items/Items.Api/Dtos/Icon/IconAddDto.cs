namespace Items.Api.Dtos.Icon;

public record IconAddDto
{
    public string? Title { get; init; }
    public string? FileName { get; set; }
    public byte[]? FileBinary { get; set; }
}
