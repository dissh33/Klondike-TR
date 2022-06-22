using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.CollectionItem;

public class AddCollectionItemCommand : IRequest<CollectionItemDto>, IWithIcon
{
    public string? Name { get; set; }
    public Guid CollectionId { get; set; }
    public Guid IconId { get; set; }
}