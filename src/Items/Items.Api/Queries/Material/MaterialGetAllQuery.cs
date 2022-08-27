using Items.Api.Dtos.Materials;
using MediatR;

namespace Items.Api.Queries.Material;

public class MaterialGetAllQuery : IRequest<IEnumerable<MaterialDto>>
{
}