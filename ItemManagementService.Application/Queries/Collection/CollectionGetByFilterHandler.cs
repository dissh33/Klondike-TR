using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.Collection;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Queries.Collection;

public class CollectionGetByFilterHandler : IRequestHandler<CollectionGetByFilterQuery, IEnumerable<CollectionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionGetByFilterHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CollectionDto>> Handle(CollectionGetByFilterQuery request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository!.GetByFilter(request, ct);

        return result.Select(collection => _mapper.Map<CollectionDto>(collection));
    }
}
