using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Collection;

public class CollectionUpdateNameCommand : IRequest<CollectionDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
