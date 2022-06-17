using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.Material;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.Material;

public class MaterialGetAllHandler : IRequestHandler<MaterialGetAllQuery, IEnumerable<MaterialDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialGetAllHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MaterialDto>> Handle(MaterialGetAllQuery request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository!.GetAll(ct);

        return result.Select(collection => _mapper.Map<MaterialDto>(collection));
    }
}