using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Material;

public class GetAllMaterialQuery : IRequest<IEnumerable<MaterialDto>>
{
}