using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Material;

public class UpdateMaterialIconCommand : IRequest<MaterialDto>, IWithIcon
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
}