namespace Items.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } //keep set; for Dapper
    public string? ExternalId { get; set; }

    public BaseEntity(Guid? id = null, string? externalId = null)
    {
        Id = id ?? Guid.NewGuid();
        ExternalId = externalId;
    }
}