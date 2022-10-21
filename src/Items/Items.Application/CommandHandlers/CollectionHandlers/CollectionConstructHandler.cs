using AutoMapper;
using Items.Api.Commands.Collection;
using Items.Api.Dtos.Collection;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Domain.Exceptions;
using MediatR;

namespace Items.Application.CommandHandlers.CollectionHandlers;

public class CollectionConstructHandler : IRequestHandler<CollectionConstructCommand, CollectionFullDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionConstructHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionFullDto?> Handle(CollectionConstructCommand request, CancellationToken ct)
    {
        var collection = ConstructCollectionForInsert(request);

        var collectionResult = await _uow.CollectionRepository.Insert(collection, ct);
        var collectionIconResult = await _uow.IconRepository.Insert(collection.Icon!, ct);

        if (collectionResult is null)
        {
            _uow.Rollback();
            return null;
        }

        var collectionItemsIconResult = (await _uow.IconRepository.BulkInsert(collection.Items.Select(collectionItem => collectionItem.Icon!), ct)).ToList();
        var collectionItemsResult = (await _uow.CollectionItemRepository.BulkInsert(collection.Items, ct)).ToList();

        if (collectionIconResult is not null)
        {
            collectionResult.AddIcon(
                collectionIconResult.Title,
                collectionIconResult.FileBinary,
                collectionIconResult.FileName,
                collectionIconResult.Id,
                collectionIconResult.ExternalId);
        }

        foreach (var item in collectionItemsResult)
        {
            var icon = collectionItemsIconResult.FirstOrDefault(icon => icon.Id == item.IconId);

            if (icon is null) continue;

            item.AddIcon(
                icon.Title,
                icon.FileBinary,
                icon.FileName,
                icon.Id,
                icon.ExternalId);
        }

        try
        {
            collectionResult.Fill(collectionItemsResult);
            _uow.Commit();
        }
        catch (WrongCollectionItemsNumberException)
        {
            _uow.Rollback();
            throw;
        }

        return _mapper.Map<CollectionFullDto>(collectionResult);
    }

    private static Collection ConstructCollectionForInsert(CollectionConstructCommand request)
    {
        var status = ItemStatus.Available;

        if (request.Status != null)
        {
            status = (ItemStatus) request.Status;
        }

        var collectionId = Guid.NewGuid();
        var collectionItems = new List<CollectionItem>();

        foreach (var item in request.Items)
        {
            var collectionItem = new CollectionItem(item.Name, collectionId);

            collectionItem.AddIcon(item.Icon.Title, item.Icon.FileBinary, item.Icon.FileName);
            collectionItems.Add(collectionItem);
        }

        var collection = new Collection(
            request.Name,
            collectionItems,
            status,
            id: collectionId
        );

        collection.AddIcon(request.Icon.Title, request.Icon.FileBinary, request.Icon.FileName);
        return collection;
    }
}