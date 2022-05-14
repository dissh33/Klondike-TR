namespace ItemManagementService.Api.Dtos;

public record CollectionDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public Guid? IconId { get; init; }
    public int? Status { get; init; }
}
