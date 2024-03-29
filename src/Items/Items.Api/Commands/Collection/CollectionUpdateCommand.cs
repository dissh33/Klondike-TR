﻿using Items.Api.Dtos.Collection;
using MediatR;

namespace Items.Api.Commands.Collection;

public class CollectionUpdateCommand : IRequest<CollectionDto>, IHaveIcon
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid IconId { get; set; }
    public int? Status { get; set; }
}
