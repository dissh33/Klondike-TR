using ItemManagementService.Domain.Enums;

namespace ItemManagementService.Domain.Entities;

public class Collection : BaseEntity, ITradableItem
{
    public Guid IconId { get; set; }
    public string? Name { get; set; }
    public ItemStatus Status { get; set; }
    public DateTime DateAdded { get; set; }
}