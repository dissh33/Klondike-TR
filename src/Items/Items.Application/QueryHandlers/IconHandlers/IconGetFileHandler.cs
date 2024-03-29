﻿using AutoMapper;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.Icon;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.QueryHandlers.IconHandlers;

public class IconGetFileHandler : IRequestHandler<IconGetFileQuery, IconFileDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IconGetFileHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IconFileDto?> Handle(IconGetFileQuery request, CancellationToken ct)
    {
        var result = await _uow.IconRepository.GetFile(request.Id, ct);

        return _mapper.Map<IconFileDto>(result);
    }
}
