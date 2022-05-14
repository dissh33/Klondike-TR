using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.Material;

public class MaterialUpdateIconCommand : IRequest<MaterialDto>, IHaveIcon
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
}