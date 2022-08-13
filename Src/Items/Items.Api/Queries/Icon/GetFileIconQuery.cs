using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Icon;

public class GetFileIconQuery : IRequest<IconFileDto>
{
    public GetFileIconQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
