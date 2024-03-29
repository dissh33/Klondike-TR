﻿using Items.Api.Dtos.Materials;
using MediatR;

namespace Items.Api.Commands.Material;

public class MaterialAddCommand : IRequest<MaterialDto>, IHaveIcon
{
    public string? Name { get; set; }
    public Guid IconId { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}