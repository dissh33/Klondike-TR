using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.Collection;

public class CollectionUpdateStatusCommand : IRequest<CollectionDto>
{
    public Guid Id { get; set; }
    public int Status { get; set; }
}
