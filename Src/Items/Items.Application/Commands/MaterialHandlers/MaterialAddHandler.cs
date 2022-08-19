using AutoMapper;
using Items.Api.Commands.Material;
using Items.Api.Dtos.Materials;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Domain.Enums;
using MediatR;

namespace Items.Application.Commands.MaterialHandlers;

public class MaterialAddHandler : IRequestHandler<MaterialAddCommand, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialAddHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialDto> Handle(MaterialAddCommand request, CancellationToken ct)
    {
        var status = ItemStatus.Available;
        var type = MaterialType.Default;

        if (request.Status != null)
        {
            status = (ItemStatus)request.Status;
        }

        if (request.Type != null)
        {
            type = (MaterialType)request.Type;
        }

        var material = new Material(
            request.Name,
            type,
            status
        );

        material.AddIcon(request.IconId);

        var result = await _uow.MaterialRepository.Insert(material, ct);

        _uow.Commit();

        return _mapper.Map<MaterialDto>(result);
    }
}
