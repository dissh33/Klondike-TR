using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.CollectionItem;

public class CollectionItemUpdateIconCommand : IRequest<CollectionItemDto>, IWithIcon
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
}