using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Material;

public class MaterialGetAllQuery : IRequest<IEnumerable<MaterialDto>>
{
}