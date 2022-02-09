using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.Collection;

public class CollectionUpdateCommand : IRequest<CollectionDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? IconId { get; set; }
    public int? Status { get; set; }
}
