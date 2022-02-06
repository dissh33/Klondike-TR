using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.Collection;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Queries.Collection;

public class CollectionGetAllHandler : IRequestHandler<CollectionGetAllQuery, IEnumerable<CollectionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionGetAllHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CollectionDto>> Handle(CollectionGetAllQuery request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository!.GetAll(ct);

        return result.Select(collection => _mapper.Map<CollectionDto>(collection));
    }
}
