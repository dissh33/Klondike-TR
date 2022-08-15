using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries;

public class GetAllTradableItemsHandler : IRequestHandler<GetAllTradableItemsQuery, IEnumerable<TradableItemDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllTradableItemsHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TradableItemDto>> Handle(GetAllTradableItemsQuery request, CancellationToken ct)
    {
        var materials = await _uow.MaterialRepository.GetAllActive(ct);
        var collections = await _uow.CollectionRepository.GetAllActive(ct);

        var materialsDtos = materials.Select(material => _mapper.Map<TradableItemDto>(material));
        var collectionDtos = collections.Select(collection => _mapper.Map<TradableItemDto>(collection));

        var result = materialsDtos.Union(collectionDtos);

        return result;
    }
}