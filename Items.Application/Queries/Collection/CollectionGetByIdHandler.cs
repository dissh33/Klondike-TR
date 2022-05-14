using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.Collection;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Queries.Collection;

public class CollectionGetByIdHandler : IRequestHandler<CollectionGetByIdQuery, CollectionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionDto> Handle(CollectionGetByIdQuery request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository!.GetById(request.Id, ct);

        return _mapper.Map<CollectionDto>(result);
    }
}