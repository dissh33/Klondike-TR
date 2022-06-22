using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Icon;

public class GetByIdIconQuery : IRequest<IconDto>
{
    public GetByIdIconQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}