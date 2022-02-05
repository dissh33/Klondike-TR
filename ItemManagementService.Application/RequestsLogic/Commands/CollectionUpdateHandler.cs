using AutoMapper;
using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Commands;

public class CollectionUpdateHandler : IRequestHandler<CollectionUpdateCommand, CollectionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionUpdateHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionDto> Handle(CollectionUpdateCommand request, CancellationToken ct)
    {
        var entity = new Collection(
            request.Name,
            iconId: request.IconId,
            id: request.Id
        );

        var result = await _uow.CollectionRepository!.Update(entity, ct);

        _uow.Commit();

        return _mapper.Map<CollectionDto>(result);
    }
}
