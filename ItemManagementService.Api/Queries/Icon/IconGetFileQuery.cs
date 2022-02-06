using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries.Icon;

public class IconGetFileQuery : IRequest<IconFileDto>
{
    public IconGetFileQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
