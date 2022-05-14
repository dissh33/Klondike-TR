using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.CollectionItem;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Queries.CollectionItem;

public class CollectionItemGetByIdHandler : IRequestHandler<CollectionItemGetByIdQuery, CollectionItemDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionItemGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionItemDto> Handle(CollectionItemGetByIdQuery request, CancellationToken ct)
    {
        var result = await _uow.CollectionItemRepository!.GetById(request.Id, ct);

        return _mapper.Map<CollectionItemDto>(result);
    }
}