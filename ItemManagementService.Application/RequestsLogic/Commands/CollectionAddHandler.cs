﻿using AutoMapper;
using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Commands;

public class CollectionAddHandler : IRequestHandler<CollectionAddCommand, CollectionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionAddHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionDto> Handle(CollectionAddCommand request, CancellationToken ct)
    {
        var entity = new Collection(
            request.Name,
            request.IconId
        );

        var result = await _uow.CollectionRepository!.Insert(entity, ct);

        _uow.Commit();

        return _mapper.Map<CollectionDto>(result);
    }
}