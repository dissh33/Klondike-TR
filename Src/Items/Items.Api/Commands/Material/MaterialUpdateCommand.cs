using Items.Api.Dtos.Materials;
using MediatR;

namespace Items.Api.Commands.Material;

public class MaterialUpdateCommand : IRequest<MaterialDto>, IHaveIcon
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
    public string? Name { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}