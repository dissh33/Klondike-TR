﻿using AutoMapper;
using Items.Api.Commands.CollectionItem;
using Items.Api.Dtos;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.CollectionItem;

public class CollectionItemUpdateHandler : IRequestHandler<CollectionItemUpdateCommand, CollectionItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemUpdateHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemDto> Handle(CollectionItemUpdateCommand request, CancellationToken ct)
    {
        var entity = new Domain.Entities.CollectionItem(
            request.Name,
            collectionId: request.CollectionId,
            iconId: request.IconId,
            id: request.Id
        );

        var result = await _uow.CollectionItemRepository!.Update(entity, ct);

        _uow.Commit();

        return _mapper.Map<CollectionItemDto>(result);
    }
}