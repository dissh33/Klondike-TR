using Items.Api.Dtos.Icon;
using MediatR;

namespace Items.Api.Queries.Icon;

public class IconGetAllQuery : IRequest<IEnumerable<IconDto>>
{

}