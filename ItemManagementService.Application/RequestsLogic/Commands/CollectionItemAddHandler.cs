using AutoMapper;
using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Commands;

public class CollectionItemAddHandler : IRequestHandler<CollectionItemAddCommand, CollectionItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemAddHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemDto> Handle(CollectionItemAddCommand request, CancellationToken ct)
    {
        var entity = new CollectionItem(
            request.Name,
            request.CollectionId,
            request.IconId
        );

        var result = await _uow.CollectionItemRepository!.Insert(entity, ct);

        _uow.Commit();

        return _mapper.Map<CollectionItemDto>(result);
    }
}