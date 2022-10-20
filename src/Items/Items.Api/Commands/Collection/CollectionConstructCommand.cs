using Items.Api.Dtos.Collection;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Dtos.Icon;
using MediatR;

namespace Items.Api.Commands.Collection;

public record CollectionConstructCommand : IRequest<CollectionFullDto>
{
    public string? Name { get; init; }
    public IconAddDto Icon { get; init; } = new();
    public int? Status { get; init; }
    public IEnumerable<CollectionItemAddDto> Items { get; init; } = Enumerable.Empty<CollectionItemAddDto>();
}
