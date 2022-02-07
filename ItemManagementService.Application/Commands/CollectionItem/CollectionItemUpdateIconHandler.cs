using AutoMapper;
using ItemManagementService.Api.Commands.CollectionItem;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Commands.CollectionItem;

public class CollectionItemUpdateIconHandler : IRequestHandler<CollectionItemUpdateIconCommand, CollectionItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemUpdateIconHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemDto> Handle(CollectionItemUpdateIconCommand request, CancellationToken ct)
    {
        var result = await _uow.CollectionItemRepository!.UpdateIcon(request.Id, request.IconId, ct);

        _uow.Commit();

        return _mapper.Map<CollectionItemDto>(result);
    }
}
