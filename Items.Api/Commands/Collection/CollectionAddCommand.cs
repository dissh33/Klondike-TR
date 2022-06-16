using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Collection;

public class CollectionAddCommand : IRequest<CollectionDto>, IHaveIcon
{
    public string? Name { get; set; }
    public Guid IconId { get; set; }
    public int? Status { get; set; }
}
