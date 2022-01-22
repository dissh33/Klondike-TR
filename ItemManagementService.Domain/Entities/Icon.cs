namespace ItemManagementService.Domain.Entities;

public class Icon : BaseEntity
{
    public string? Title { get; }
    public byte[]? FileBinary { get; }
    public string? FileName { get; }

    public Icon()
    {
        
    }

    public Icon(string? title, byte[]? fileBinary, string? fileName, Guid? id = null, string? externalId = null)
    {
        Id = id ?? Guid.NewGuid();
        ExternalId = externalId;
        Title = title;
        FileBinary = fileBinary ?? throw new ArgumentNullException();
        FileName = fileName ?? throw new ArgumentNullException();
    }
}