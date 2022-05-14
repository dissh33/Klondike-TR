﻿using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.CollectionItem;

public class CollectionItemUpdateCommand : IRequest<CollectionItemDto>, IHaveIcon
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid CollectionId { get; set; }
    public Guid IconId { get; set; }
}