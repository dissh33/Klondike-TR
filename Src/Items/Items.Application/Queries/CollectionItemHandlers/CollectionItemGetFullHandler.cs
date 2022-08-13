using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.CollectionItem;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.CollectionItemHandlers;

public class CollectionItemGetFullHandler : IRequestHandler<CollectionItemGetFullQuery, CollectionItemFullDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemGetFullHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemFullDto> Handle(CollectionItemGetFullQuery request, CancellationToken ct)
    {
        var collectionItem = await _uow.CollectionItemRepository.GetById(request.Id, ct);
        var icon = await _uow.IconRepository.GetById(collectionItem.IconId, ct);

        collectionItem.AddIcon(
            icon.Title,
            icon.FileBinary,
            icon.FileName,
            icon.Id,
            icon.ExternalId);

        return _mapper.Map<CollectionItemFullDto>(collectionItem);
    }
}