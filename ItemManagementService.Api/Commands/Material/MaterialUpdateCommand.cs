﻿using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.Material;

public class MaterialUpdateCommand : IRequest<MaterialDto>
{
    public Guid Id { get; set; }
    public Guid? IconId { get; set; }
    public string? Name { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}