using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.Material;

public class MaterialUpdateStatusCommand : IRequest<MaterialDto>
{
    public Guid Id { get; set; }
    public int Status { get; set; }
}