using AutoMapper;
using ItemManagementService.Api.Commands.Collection;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Commands.Collection;

public class CollectionUpdateNameHandler : IRequestHandler<CollectionUpdateNameCommand, CollectionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionUpdateNameHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionDto> Handle(CollectionUpdateNameCommand request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository!.UpdateName(request.Id, request.Name, ct);

        _uow.Commit();

        return _mapper.Map<CollectionDto>(result);
    }
}
