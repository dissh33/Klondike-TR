﻿using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.Collection;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.CollectionHandlers;

public class CollectionGetAllHandler : IRequestHandler<CollectionGetAllQuery, IEnumerable<CollectionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionGetAllHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CollectionDto>> Handle(CollectionGetAllQuery request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository.GetAll(ct);

        return result.Select(collection => _mapper.Map<CollectionDto>(collection));
    }
}