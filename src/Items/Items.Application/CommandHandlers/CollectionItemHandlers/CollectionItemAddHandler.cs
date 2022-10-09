using AutoMapper;
using Items.Api.Commands.CollectionItem;
using Items.Api.Dtos.CollectionItem;
using Items.Application.Contracts;
using Items.Domain.Entities;
using MediatR;

namespace Items.Application.CommandHandlers.CollectionItemHandlers;

public class CollectionItemAddHandler : IRequestHandler<CollectionItemAddCommand, CollectionItemDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemAddHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemDto?> Handle(CollectionItemAddCommand request, CancellationToken ct)
    {
        var collectionItem = new CollectionItem(
            request.Name,
            request.CollectionId,
            request.IconId
        );

        collectionItem.AddIcon(request.IconId);

        var result = await _uow.CollectionItemRepository.Insert(collectionItem, ct);

        _uow.Commit();

        return _mapper.Map<CollectionItemDto>(result);
    }
}