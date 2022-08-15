using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.CollectionItem;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.CollectionItemHandlers;

public class CollectionItemGetFullByCollectionHandler : IRequestHandler<CollectionItemGetFullByCollectionQuery, IEnumerable<CollectionItemFullDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemGetFullByCollectionHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CollectionItemFullDto>> Handle(CollectionItemGetFullByCollectionQuery request, CancellationToken ct)
    {
        var collectionItems = (await _uow.CollectionItemRepository.GetByCollection(request.CollectionId, ct)).ToList();

        var iconsIds = collectionItems.Select(item => item.IconId);
        var icons = (await _uow.IconRepository.GetRange(iconsIds, ct)).ToList();

        foreach (var collectionItem in collectionItems)
        {
            var currentIcon = icons.First(icon => icon.Id == collectionItem.Id);

            collectionItem.AddIcon(
                currentIcon.Title,
                currentIcon.FileBinary,
                currentIcon.FileName,
                currentIcon.Id);
        }

        return collectionItems.Select(collectionItem => _mapper.Map<CollectionItemFullDto>(collectionItem));
    }
}
