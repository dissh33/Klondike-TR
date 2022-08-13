using AutoMapper;
using Items.Api.Commands.CollectionItem;
using Items.Api.Dtos;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.CollectionItemHandlers;

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
        var result = await _uow.CollectionItemRepository.UpdateIcon(request.Id, request.IconId, ct);

        _uow.Commit();

        return _mapper.Map<CollectionItemDto>(result);
    }
}
