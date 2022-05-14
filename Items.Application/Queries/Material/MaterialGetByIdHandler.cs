using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.Material;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Queries.Material;

public class MaterialGetByIdHandler : IRequestHandler<MaterialGetByIdQuery, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialDto> Handle(MaterialGetByIdQuery request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository!.GetById(request.Id, ct);

        return _mapper.Map<MaterialDto>(result);
    }
}