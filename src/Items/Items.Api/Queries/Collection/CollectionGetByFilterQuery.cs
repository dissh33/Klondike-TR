using Items.Api.Dtos.Collection;
using MediatR;

namespace Items.Api.Queries.Collection;

public class CollectionGetByFilterQuery : IRequest<IEnumerable<CollectionDto>>
{
    public string? Name { get; set; }
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}