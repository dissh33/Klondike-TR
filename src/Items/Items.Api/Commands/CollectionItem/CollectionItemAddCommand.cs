﻿using Items.Api.Dtos.CollectionItem;
using MediatR;

namespace Items.Api.Commands.CollectionItem;

public class CollectionItemAddCommand : IRequest<CollectionItemDto>, IHaveIcon
{
    public string? Name { get; set; }
    public Guid CollectionId { get; set; }
    public Guid IconId { get; set; }
}