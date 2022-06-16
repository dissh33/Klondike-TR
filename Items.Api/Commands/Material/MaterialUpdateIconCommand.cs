using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Material;

public class MaterialUpdateIconCommand : IRequest<MaterialDto>, IHaveIcon
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
}