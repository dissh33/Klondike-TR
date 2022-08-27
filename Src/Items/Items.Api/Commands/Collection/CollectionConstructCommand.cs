using Items.Api.Dtos.Collection;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Dtos.Icon;
using MediatR;

namespace Items.Api.Commands.Collection;

public class CollectionConstructCommand : IRequest<CollectionFullDto>
{
    public string? Name { get; set; }
    public IconAddDto Icon { get; set; } = new();
    public int? Status { get; set; }
    public IEnumerable<CollectionItemAddDto> Items { get; set; } = Enumerable.Empty<CollectionItemAddDto>();
}
