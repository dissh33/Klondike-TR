namespace ItemManagementService.Api.Dtos;

public class CollectionItemDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? CollectionId { get; set; }
    public Guid? IconId { get; set; }
}