using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Material;

public class GetByIdMaterialQuery : IRequest<MaterialDto>
{
    public GetByIdMaterialQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}