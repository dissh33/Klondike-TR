using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.Collection;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.CollectionHandlers;

public class CollectionGetFullHandler : IRequestHandler<CollectionGetFullQuery, CollectionFullDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CollectionGetFullHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CollectionFullDto> Handle(CollectionGetFullQuery request, CancellationToken ct)
    {
        var collection = await _uow.CollectionRepository.GetById(request.Id, ct);
        var icon = await _uow.IconRepository.GetById(collection.IconId, ct);

        collection.AddIcon(
            icon.Title,
            icon.FileBinary,
            icon.FileName,
            icon.Id,
            icon.ExternalId);

        return _mapper.Map<CollectionFullDto>(collection);
    }
}
