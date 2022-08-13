using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.Collection;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.CollectionHandlers;

public class CollectionGetFullHandler : IRequestHandler<CollectionGetFullQuery, CollectionFullDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionGetFullHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionFullDto> Handle(CollectionGetFullQuery request, CancellationToken ct)
    {
        var collection = await _uow.CollectionRepository.GetById(request.Id, ct);
        var collectionIcon = await _uow.IconRepository.GetById(collection.IconId, ct);

        collection.AddIcon(
            collectionIcon.Title,
            collectionIcon.FileBinary,
            collectionIcon.FileName,
            collectionIcon.Id,
            collectionIcon.ExternalId);

        var collectionItems = (await _uow.CollectionItemRepository.GetByCollection(collection.Id, ct)).ToList();
        var collectionItemsIcons = (await _uow.IconRepository.GetRange(collectionItems.Select(item => item.IconId), ct)).ToList();

        foreach (var item in collectionItems)
        {
            var collectionItemIcon = collectionItemsIcons.FirstOrDefault(icon => icon.Id == item.IconId);

            if (collectionItemIcon is null) continue;

            item.AddIcon(
                collectionItemIcon.Title,
                collectionItemIcon.FileBinary,
                collectionItemIcon.FileName,
                collectionItemIcon.Id,
                collectionItemIcon.ExternalId);
        }

        collection.Fill(collectionItems);

        return _mapper.Map<CollectionFullDto>(collection);
    }
}
