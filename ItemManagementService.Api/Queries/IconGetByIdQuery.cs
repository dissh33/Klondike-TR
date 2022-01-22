using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Queries;

public class IconGetByIdQuery : IRequest<IconDto>
{
    public IconGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}