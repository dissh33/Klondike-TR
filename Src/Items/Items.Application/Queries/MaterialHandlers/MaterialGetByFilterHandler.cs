using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.Material;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.MaterialHandlers;

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
        var result = await _uow.MaterialRepository.GetByFilter(request, ct);

        return result.Select(material => _mapper.Map<MaterialDto>(material));
    }
}