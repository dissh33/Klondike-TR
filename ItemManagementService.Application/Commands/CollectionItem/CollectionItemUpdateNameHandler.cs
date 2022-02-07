using AutoMapper;
using ItemManagementService.Api.Commands.CollectionItem;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Commands.CollectionItem;

public class CollectionItemUpdateNameHandler : IRequestHandler<CollectionItemUpdateNameCommand, CollectionItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemUpdateNameHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemDto> Handle(CollectionItemUpdateNameCommand request, CancellationToken ct)
    {
        var result = await _uow.CollectionItemRepository!.UpdateName(request.Id, request.Name, ct);

        _uow.Commit();

        return _mapper.Map<CollectionItemDto>(result);
    }
}