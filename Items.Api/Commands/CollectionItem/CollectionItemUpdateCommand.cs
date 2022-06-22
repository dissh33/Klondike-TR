using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.CollectionItem;

public class CollectionItemUpdateCommand : IRequest<CollectionItemDto>, IWithIcon
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid CollectionId { get; set; }
    public Guid IconId { get; set; }
}