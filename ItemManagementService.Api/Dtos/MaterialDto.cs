namespace ItemManagementService.Api.Dtos;

public class MaterialDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int? Type { get; set; }
    public Guid? IconId { get; set; }
    public int? Status { get; set; }
}