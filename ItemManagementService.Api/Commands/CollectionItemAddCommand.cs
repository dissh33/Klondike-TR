using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands;

public class CollectionItemAddCommand : IRequest<CollectionItemDto>
{
    public string? Name { get; set; }
    public Guid? CollectionId { get; set; }
    public Guid? IconId { get; set; }
}