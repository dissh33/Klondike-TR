using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries;

public class IconGetAllQuery : IRequest<IEnumerable<IconDto>>
{

}