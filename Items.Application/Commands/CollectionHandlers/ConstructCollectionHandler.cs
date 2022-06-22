using AutoMapper;
using Items.Api.Commands.Collection;
using Items.Api.Dtos;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Domain.Enums;
using MediatR;

namespace Items.Application.Commands.CollectionHandlers;

public class ConstructCollectionHandler : IRequestHandler<ConstructCollectionCommand, CollectionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ConstructCollectionHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionDto> Handle(ConstructCollectionCommand request, CancellationToken ct)
    {
        var status = ItemStatus.Active;

        if (request.Status != null)
        {
            status = (ItemStatus)request.Status;
        }

        var collectionId = Guid.NewGuid();

        var collectionItemEntities = new List<CollectionItem>();

        foreach (var item in request.Items)
        {
            var collectionItemEntity = new CollectionItem(item.Name, collectionId);

            collectionItemEntity.AddIcon(item.Icon.Title, item.Icon.FileBinary, item.Icon.FileName);

            collectionItemEntities.Add(collectionItemEntity);
        }

        var collectionEntity = new Collection(
            request.Name,
            collectionItemEntities,
            status,
            id: collectionId
        );

        collectionEntity.AddIcon(request.Icon.Title, request.Icon.FileBinary, request.Icon.FileName);

        var collectionIconResult = await _uow.IconRepository.Insert(collectionEntity.Icon!, ct);        
        var collectionResult = await _uow.CollectionRepository.Insert(collectionEntity, ct);

        var collectionItemsIconResult = await _uow.IconRepository.BulkInsert(collectionEntity.Items.Select(x => x.Icon!), ct);
        var collectionItemsResult = await _uow.CollectionItemRepository.BulkInsert(collectionEntity.Items, ct);

        _uow.Commit();
        
        return _mapper.Map<CollectionDto>(collectionResult);
    }
}