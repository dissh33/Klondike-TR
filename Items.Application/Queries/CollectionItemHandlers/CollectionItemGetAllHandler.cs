using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.CollectionItem;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.CollectionItemHandlers;

public class CollectionItemGetAllHandler : IRequestHandler<CollectionItemGetAllQuery, IEnumerable<CollectionItemDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemGetAllHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CollectionItemDto>> Handle(CollectionItemGetAllQuery request, CancellationToken ct)
    {
        var result = await _uow.CollectionItemRepository.GetAll(ct);

        return result.Select(collectionItem => _mapper.Map<CollectionItemDto>(collectionItem));
    }
}
