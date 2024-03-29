﻿using AutoMapper;
using Items.Api.Commands.Collection;
using Items.Api.Dtos.Collection;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.CommandHandlers.CollectionHandlers;

public class CollectionUpdateStatusHandler : IRequestHandler<CollectionUpdateStatusCommand, CollectionDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionUpdateStatusHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionDto?> Handle(CollectionUpdateStatusCommand request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository.UpdateStatus(request.Id, request.Status, ct);

        _uow.Commit();

        return _mapper.Map<CollectionDto>(result);
    }
}
