﻿using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Commands.Material;

public class UpdateMaterialCommand : IRequest<MaterialDto>, IWithIcon
{
    public Guid Id { get; set; }
    public Guid IconId { get; set; }
    public string? Name { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}