using Items.Api.Dtos.Icon;
using MediatR;

namespace Items.Api.Queries.Icon;

public class IconGetByIdQuery : IRequest<IconDto>
{
    public IconGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}