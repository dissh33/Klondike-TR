using AutoMapper;
using ItemManagementService.Api.Commands.Material;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Commands.Material;

public class MaterialUpdateStatusHandler : IRequestHandler<MaterialUpdateStatusCommand, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialUpdateStatusHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialDto> Handle(MaterialUpdateStatusCommand request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository!.UpdateStatus(request.Id, request.Status, ct);

        _uow.Commit();

        return _mapper.Map<MaterialDto>(result);
    }
}