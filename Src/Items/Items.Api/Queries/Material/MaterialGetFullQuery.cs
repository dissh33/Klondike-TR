using Items.Api.Dtos.Materials;
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