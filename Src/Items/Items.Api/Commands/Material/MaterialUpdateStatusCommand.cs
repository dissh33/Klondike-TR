using Items.Api.Dtos.Materials;
using MediatR;

namespace Items.Api.Commands.Material;

public class MaterialUpdateStatusCommand : IRequest<MaterialDto>
{
    public Guid Id { get; set; }
    public int Status { get; set; }
}