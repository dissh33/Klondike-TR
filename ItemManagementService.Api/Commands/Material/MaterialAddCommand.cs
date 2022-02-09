using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.Material;

public class MaterialAddCommand : IRequest<MaterialDto>, IHaveIcon
{
    public string? Name { get; set; }
    public Guid IconId { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}