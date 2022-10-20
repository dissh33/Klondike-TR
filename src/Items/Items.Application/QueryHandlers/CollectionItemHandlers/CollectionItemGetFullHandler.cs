using AutoMapper;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.QueryHandlers.CollectionItemHandlers;

public class CollectionItemGetFullHandler : IRequestHandler<CollectionItemGetFullQuery, CollectionItemFullDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemGetFullHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemFullDto?> Handle(CollectionItemGetFullQuery request, CancellationToken ct)
    {
        var collectionItem = await _uow.CollectionItemRepository.GetById(request.Id, ct);
        if (collectionItem is null) return null;

        var icon = await _uow.IconRepository.GetById(collectionItem.IconId, ct);
        
        if (icon is not null)
        {
            collectionItem.AddIcon(
                icon.Title,
                icon.FileBinary,
                icon.FileName,
                icon.Id,
                icon.ExternalId);
        }

        return _mapper.Map<CollectionItemFullDto>(collectionItem);
    }
}