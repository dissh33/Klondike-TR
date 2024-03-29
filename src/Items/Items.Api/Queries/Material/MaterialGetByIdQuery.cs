﻿using Items.Api.Dtos.Materials;
using MediatR;

namespace Items.Api.Queries.Material;

public class MaterialGetByIdQuery : IRequest<MaterialDto>
{
    public MaterialGetByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}