namespace Items.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; }
    public string? ExternalId { get; }

    public BaseEntity(Guid? id = null, string? externalId = null)
    {
        Id = id ?? Guid.NewGuid();
        ExternalId = externalId;
    }
}