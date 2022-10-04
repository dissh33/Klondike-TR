using AutoMapper;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.QueryHandlers.CollectionItemHandlers;

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
        var result = await _uow.CollectionItemRepository.GetById(request.Id, ct);

        return _mapper.Map<CollectionItemDto>(result);
    }
}