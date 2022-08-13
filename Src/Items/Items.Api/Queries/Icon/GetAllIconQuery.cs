using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Icon;

public class GetAllIconQuery : IRequest<IEnumerable<IconDto>>
{

}