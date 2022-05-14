using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.Material;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Queries.Material;

public class MaterialGetByFilterHandler : IRequestHandler<MaterialGetByFilterQuery, IEnumerable<MaterialDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialGetByFilterHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MaterialDto>> Handle(MaterialGetByFilterQuery request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository!.GetByFilter(request, ct);

        return result.Select(collection => _mapper.Map<MaterialDto>(collection));
    }
}