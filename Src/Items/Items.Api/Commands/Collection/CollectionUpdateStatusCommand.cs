using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Collection;

public class CollectionUpdateStatusCommand : IRequest<CollectionDto>
{
    public Guid Id { get; set; }
    public int Status { get; set; }
}
