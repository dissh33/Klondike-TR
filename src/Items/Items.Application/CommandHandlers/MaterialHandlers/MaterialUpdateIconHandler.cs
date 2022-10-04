using AutoMapper;
using Items.Api.Commands.Material;
using Items.Api.Dtos.Materials;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.CommandHandlers.MaterialHandlers;

public class MaterialUpdateIconHandler : IRequestHandler<MaterialUpdateIconCommand, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialUpdateIconHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialDto> Handle(MaterialUpdateIconCommand request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository.UpdateIcon(request.Id, request.IconId, ct);

        _uow.Commit();

        return _mapper.Map<MaterialDto>(result);
    }
}
