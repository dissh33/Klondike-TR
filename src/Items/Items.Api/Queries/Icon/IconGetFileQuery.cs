using Items.Api.Dtos.Icon;
using MediatR;

namespace Items.Api.Queries.Icon;

public class IconGetFileQuery : IRequest<IconFileDto>
{
    public IconGetFileQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
