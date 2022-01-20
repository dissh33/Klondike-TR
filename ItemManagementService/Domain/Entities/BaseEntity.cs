namespace ItemManagementService.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public string? ExternalId { get; set; }
}