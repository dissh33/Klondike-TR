namespace ItemManagementService.Domain;

public class BaseEntity
{
    public Guid Id { get; set; }
    public string? ExternalId { get; set; }
}