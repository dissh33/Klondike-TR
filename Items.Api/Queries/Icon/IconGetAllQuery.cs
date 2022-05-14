using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries.Icon;

public class IconGetAllQuery : IRequest<IEnumerable<IconDto>>
{

}