namespace Items.Domain.Entities;

public class Icon : BaseEntity
{
    public string? Title { get; private set; }

    public byte[]? FileBinary { get; }
    public string? FileName { get; }

    private Icon()
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

    public Icon(
        Guid? id,
        string? externalId = null)
        : base(id, externalId)
    {

    }
}