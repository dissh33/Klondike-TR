using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Collection;

public class CollectionConstructCommand : IRequest<CollectionDto>
{
    public string? Name { get; set; }
    public IconAddDto Icon { get; set; } = new();
    public int? Status { get; set; }
    public IEnumerable<CollectionItemAddDto> Items { get; set; } = Enumerable.Empty<CollectionItemAddDto>();
}
