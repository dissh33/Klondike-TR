using AutoMapper;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.QueryHandlers.CollectionItemHandlers;

public class CollectionItemGetByCollectionHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CollectionItemGetByCollectionQuery, IEnumerable<CollectionItemDto>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CollectionItemDto>> Handle(CollectionItemGetByCollectionQuery request, CancellationToken ct)
    {
        var result = await _uow.CollectionItemRepository.GetByCollection(request.CollectionId, ct);

        return result.Select(collectionItem => _mapper.Map<CollectionItemDto>(collectionItem));
    }
}
