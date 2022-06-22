using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.CollectionItem;

public class CollectionItemAddCommand : IRequest<CollectionItemDto>, IWithIcon
{
    public string? Name { get; set; }
    public Guid CollectionId { get; set; }
    public Guid IconId { get; set; }
}