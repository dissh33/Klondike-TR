using AutoMapper;
using Items.Api.Commands.CollectionItem;
using Items.Api.Dtos.CollectionItem;
using Items.Application.Contracts;
using Items.Domain.Entities;
using MediatR;

namespace Items.Application.CommandHandlers.CollectionItemHandlers;

public class CollectionItemUpdateHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CollectionItemUpdateCommand, CollectionItemDto?>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;

    public async Task<CollectionItemDto?> Handle(CollectionItemUpdateCommand request, CancellationToken ct)
    {
        var collectionItem = new CollectionItem(
            request.Name,
            collectionId: request.CollectionId,
            id: request.Id
        );

        if (request.IconId != Guid.Empty)
        {
            collectionItem.AddIcon(request.IconId);
        }

        var result = await _uow.CollectionItemRepository.Update(collectionItem, ct);

        _uow.Commit();

        return _mapper.Map<CollectionItemDto>(result);
    }
}