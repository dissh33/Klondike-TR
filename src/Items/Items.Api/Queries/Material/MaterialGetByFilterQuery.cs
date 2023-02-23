using Items.Api.Dtos.Materials;
using MediatR;

namespace Items.Api.Queries.Material;

public class MaterialGetByFilterQuery : IRequest<IEnumerable<MaterialDto>>
{
    public string? Name { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}