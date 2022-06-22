using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Material;

public class AddMaterialCommand : IRequest<MaterialDto>, IWithIcon
{
    public string? Name { get; set; }
    public Guid IconId { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}