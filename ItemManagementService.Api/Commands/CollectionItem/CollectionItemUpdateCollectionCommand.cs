﻿using ItemManagementService.Api.Dtos;
using MediatR;

namespace ItemManagementService.Api.Commands.CollectionItem;

public class CollectionItemUpdateCollectionCommand : IRequest<CollectionItemDto>
{
    public Guid Id { get; set; }
    public Guid? CollectionId { get; set; }
}
