namespace ItemManagementService.Domain;

public class BaseEntity
{
    public Guid Id { get; set; }
    public string? ExternalId { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public BaseEntity(string? externalId) : this()
    {
        ExternalId = externalId;
    }
}