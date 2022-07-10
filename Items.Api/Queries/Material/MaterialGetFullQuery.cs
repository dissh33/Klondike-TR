using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Material;

public class MaterialGetFullQuery : IRequest<MaterialFullDto>
{
    public MaterialGetFullQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}