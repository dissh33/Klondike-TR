using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries;
using Items.Application.Contracts;
using Items.Domain.Entities;
using MediatR;
using System.Collections.Generic;

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

        var iconsIds = materials.Select(material => material.IconId)
            .Union(collections.Select(collection => collection.IconId));

        var icons = await _uow.IconRepository.GetRange(iconsIds, ct);

        foreach (var icon in icons)
        {
            var matchedItems = materials.Where(material => material.IconId == icon.Id)
                .Union(collections.Select(collection => (ITradableItem)collection).Where(item => item.IconId == icon.Id));

            foreach (var currentItem in matchedItems)
            {
                currentItem.AddIcon(
                    icon.Title,
                    icon.FileBinary,
                    icon.FileName,
                    icon.Id);
            }
        }

        var materialsDtos = materials.Select(material => _mapper.Map<TradableItemDto>(material));
        var collectionDtos = collections.Select(collection => _mapper.Map<TradableItemDto>(collection));

        var result = materialsDtos.Union(collectionDtos);

        return result;
    }
}