﻿using AutoMapper;
using Items.Api.Commands.Material;
using Items.Api.Dtos;
using Items.Application.Contracts;
using Items.Domain.Enums;
using MediatR;

namespace Items.Application.Commands.Material;

public class MaterialUpdateHandler : IRequestHandler<MaterialUpdateCommand, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialUpdateHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialDto> Handle(MaterialUpdateCommand request, CancellationToken ct)
    {
        var status = ItemStatus.Active;
        var type = MaterialType.Default;

        if (request.Status != null)
        {
            status = (ItemStatus)request.Status;
        }

        if (request.Type != null)
        {
            type = (MaterialType)request.Type;
        }

        var entity = new Domain.Entities.Material(
            request.Name,
            request.IconId,
            type,
            status,
            request.Id
        );

        var result = await _uow.MaterialRepository!.Update(entity, ct);

        _uow.Commit();

        return _mapper.Map<MaterialDto>(result);
    }
}