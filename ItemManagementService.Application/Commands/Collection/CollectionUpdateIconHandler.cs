using AutoMapper;
using ItemManagementService.Api.Commands.Collection;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Commands.Collection;

public class CollectionUpdateIconHandler : IRequestHandler<CollectionUpdateIconCommand, CollectionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionUpdateIconHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionDto> Handle(CollectionUpdateIconCommand request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository!.UpdateIcon(request.Id, request.IconId, ct);

        _uow.Commit();

        return _mapper.Map<CollectionDto>(result);
    }
}
