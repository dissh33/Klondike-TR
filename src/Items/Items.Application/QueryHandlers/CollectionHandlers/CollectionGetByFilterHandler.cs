using AutoMapper;
using Items.Api.Dtos.Collection;
using Items.Api.Queries.Collection;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.QueryHandlers.CollectionHandlers;

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
        var result = await _uow.CollectionRepository.GetByFilter(request, ct);

        return result.Select(collection => _mapper.Map<CollectionDto>(collection));
    }
}