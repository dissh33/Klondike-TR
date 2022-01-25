namespace ItemManagementService.Api.Dtos;

public class IconFileDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? FileName { get; set; }
    public Stream? FileStream { get; set; }
}
