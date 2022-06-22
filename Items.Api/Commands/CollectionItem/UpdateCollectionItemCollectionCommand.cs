using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.CollectionItem;

public class UpdateCollectionItemCollectionCommand : IRequest<CollectionItemDto> 
{
    public Guid Id { get; set; }
    public Guid CollectionId { get; set; }
}
