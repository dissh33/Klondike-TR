using AutoMapper;
using Items.Api.Commands.Collection;
using Items.Api.Dtos;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Domain.Enums;
using MediatR;

namespace Items.Application.Commands.CollectionHandlers;

public class CollectionConstructHandler : IRequestHandler<CollectionConstructCommand, CollectionFullDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionConstructHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionFullDto> Handle(CollectionConstructCommand request, CancellationToken ct)
    {
        var status = ItemStatus.Active;

        if (request.Status != null)
        {
            status = (ItemStatus)request.Status;
        }

        var collectionId = Guid.NewGuid();

        var collectionItems = new List<CollectionItem>();

        foreach (var item in request.Items)
        {
            var collectionItem = new CollectionItem(item.Name, collectionId);

            collectionItem.AddIcon(item.Icon.Title, item.Icon.FileBinary, item.Icon.FileName);

            collectionItems.Add(collectionItem);
        }

        var collectionEntity = new Collection(
            request.Name,
            collectionItems,
            status,
            id: collectionId
        );

        collectionEntity.AddIcon(request.Icon.Title, request.Icon.FileBinary, request.Icon.FileName);

        var collectionIconResult = await _uow.IconRepository.Insert(collectionEntity.Icon!, ct);        
        var collectionResult = await _uow.CollectionRepository.Insert(collectionEntity, ct);

        var collectionItemsIconResult = (await _uow.IconRepository.BulkInsert(collectionEntity.Items.Select(collectionItem => collectionItem.Icon!), ct)).ToList();
        var collectionItemsResult = (await _uow.CollectionItemRepository.BulkInsert(collectionEntity.Items, ct)).ToList();

        _uow.Commit();

        collectionResult.AddIcon(
            collectionIconResult.Title,
            collectionIconResult.FileBinary,
            collectionIconResult.FileName,
            collectionIconResult.Id,
            collectionIconResult.ExternalId);
        
        foreach (var item in collectionItemsResult)
        {
            var icon = collectionItemsIconResult.FirstOrDefault();

            if (icon == null) continue;

            collectionItemsIconResult.Remove(icon);

            item.AddIcon(
                icon.Title,
                icon.FileBinary,
                icon.FileName,
                icon.Id,
                icon.ExternalId);
        }

        collectionResult.Fill(collectionItemsResult);
        
        return _mapper.Map<CollectionFullDto>(collectionResult);
    }
}