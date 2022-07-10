using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.Material;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.MaterialHandlers;

public class MaterialGetFullHandler : IRequestHandler<MaterialGetFullQuery, MaterialFullDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialGetFullHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialFullDto> Handle(MaterialGetFullQuery request, CancellationToken ct)
    {
        var material = await _uow.MaterialRepository.GetById(request.Id, ct);
        var icon = await _uow.IconRepository.GetById(material.IconId, ct);

        material.AddIcon(
            icon.Title,
            icon.FileBinary,
            icon.FileName,
            icon.Id,
            icon.ExternalId);

        return _mapper.Map<MaterialFullDto>(material);
    }
}
