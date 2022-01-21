namespace ItemManagementService.Domain.Entities;

public class Icon : BaseEntity
{
    public string? Title { get; }
    public byte[]? Binary { get; }

    public Icon(string? title, byte[]? binary) : base()
    {
        Title = title;
        Binary = binary;
    }

    public Icon(string? title, byte[]? binary, string? externalId = null) : base(externalId)
    {
        Title = title;
        Binary = binary;
    }
}