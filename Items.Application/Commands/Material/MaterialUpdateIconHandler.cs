using AutoMapper;
using ItemManagementService.Api.Commands.Material;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Commands.Material;

public class MaterialUpdateIconHandler : IRequestHandler<MaterialUpdateIconCommand, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialUpdateIconHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialDto> Handle(MaterialUpdateIconCommand request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository!.UpdateIcon(request.Id, request.IconId, ct);

        _uow.Commit();

        return _mapper.Map<MaterialDto>(result);
    }
}
