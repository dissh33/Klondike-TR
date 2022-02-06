using AutoMapper;
using ItemManagementService.Api.Commands.CollectionItem;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Commands.CollectionItem;

public class CollectionItemUpdateCollectionHandler : IRequestHandler<CollectionItemUpdateCollectionCommand, CollectionItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemUpdateCollectionHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemDto> Handle(CollectionItemUpdateCollectionCommand request, CancellationToken ct)
    {
        var result = await _uow.CollectionItemRepository!.UpdateCollection(request.Id, request.CollectionId, ct);

        _uow.Commit();

        return _mapper.Map<CollectionItemDto>(result);
    }
}