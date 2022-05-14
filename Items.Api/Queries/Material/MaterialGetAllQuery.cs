using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries.Material;

public class MaterialGetAllQuery : IRequest<IEnumerable<MaterialDto>>
{
}