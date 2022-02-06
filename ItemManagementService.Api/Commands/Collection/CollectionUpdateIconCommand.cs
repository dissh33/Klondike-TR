﻿using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.Collection;

public class CollectionUpdateIconCommand : IRequest<CollectionDto>
{
    public Guid Id { get; set; }
    public Guid? IconId { get; set; }
}
