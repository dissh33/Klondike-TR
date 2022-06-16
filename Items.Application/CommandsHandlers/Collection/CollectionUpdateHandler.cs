using AutoMapper;
using Items.Api.Commands.Collection;
using Items.Api.Dtos;
using Items.Application.Contracts;
using Items.Domain.Enums;
using MediatR;

namespace Items.Application.Commands.Collection;

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
        var status = ItemStatus.Active;

        if (request.Status != null)
        {
            status = (ItemStatus)request.Status;
        }

        var entity = new Domain.Entities.Collection(
            request.Name,
            request.IconId,
            status,
            request.Id
        );

        var result = await _uow.CollectionRepository!.Update(entity, ct);

        _uow.Commit();

        return _mapper.Map<CollectionDto>(result);
    }
}
