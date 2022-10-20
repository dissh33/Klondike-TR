using AutoMapper;
using Items.Api.Dtos.Collection;
using Items.Api.Queries.Collection;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.QueryHandlers.CollectionHandlers;

public class CollectionGetByIdHandler : IRequestHandler<CollectionGetByIdQuery, CollectionDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionDto?> Handle(CollectionGetByIdQuery request, CancellationToken ct)
    {
        var result = await _uow.CollectionRepository.GetById(request.Id, ct);

        return _mapper.Map<CollectionDto>(result);
    }
}