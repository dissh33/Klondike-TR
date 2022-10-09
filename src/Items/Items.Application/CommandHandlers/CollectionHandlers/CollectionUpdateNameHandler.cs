using AutoMapper;
using Items.Api.Commands.Collection;
using Items.Api.Dtos.Collection;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.CommandHandlers.CollectionHandlers;

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
        var result = await _uow.CollectionRepository.UpdateName(request.Id, request.Name, ct);

        _uow.Commit();

        return _mapper.Map<CollectionDto>(result);
    }
}
