using Items.Domain.Entities;

namespace Items.Domain.Entities;

public class Icon : BaseEntity
{
    public string? Title { get; }
    public byte[]? FileBinary { get; }
    public string? FileName { get; }

    public Icon()
    {
        
    }

    public Icon(
        string? title, 
        byte[]? fileBinary, 
        string? fileName, 
        Guid? id = null, 
        string? externalId = null)
        : base(id, externalId)
    {
        Title = title;
        FileBinary = fileBinary;
        FileName = fileName;
    }
}