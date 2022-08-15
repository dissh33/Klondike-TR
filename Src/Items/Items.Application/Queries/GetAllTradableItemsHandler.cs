using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries;
using Items.Application.Contracts;
using Items.Domain.Entities;
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
        var materials = (await _uow.MaterialRepository.GetAllActive(ct)).ToList();
        var collections = (await _uow.CollectionRepository.GetAllActive(ct)).ToList();

        var tradableItems = materials.Union(collections.Select(collection => (ITradableItem) collection)).ToList();

        var iconsIds = tradableItems.Select(item => item.IconId);
        var icons = (await _uow.IconRepository.GetRange(iconsIds, ct)).ToList();

        foreach (var item in tradableItems)
        {
            var currentIcon = icons.First(icon => icon.Id == item.IconId);

            item.AddIcon(
                currentIcon.Title,
                currentIcon.FileBinary,
                currentIcon.FileName,
                currentIcon.Id);
        }

        var materialsDtos = materials.Select(material => _mapper.Map<TradableItemDto>(material));
        var collectionDtos = collections.Select(collection => _mapper.Map<TradableItemDto>(collection));

        var result = materialsDtos.Union(collectionDtos);

        return result;
    }
}