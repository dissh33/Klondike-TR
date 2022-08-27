namespace Items.Domain.Entities;

public interface ITradableItem
{
    public Guid Id { get; }
    public string? Name { get; }

    public Icon? Icon { get; }  
    public Guid IconId { get; }
    void AddIcon(
        string? title,
        byte[]? fileBinary,
        string? fileName,
        Guid? id = null,
        string? externalId = null);
}