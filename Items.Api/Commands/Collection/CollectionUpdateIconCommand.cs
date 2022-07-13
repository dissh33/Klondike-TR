using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Collection;

public class CollectionUpdateIconCommand : IRequest<CollectionDto>, IHaveIcon
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
}
