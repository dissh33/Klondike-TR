using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Collection;

public class UpdateCollectionCommand : IRequest<CollectionDto>, IWithIcon
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid IconId { get; set; }
    public int? Status { get; set; }
}
