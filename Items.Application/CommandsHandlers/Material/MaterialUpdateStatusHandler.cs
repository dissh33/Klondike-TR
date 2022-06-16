using AutoMapper;
using Items.Api.Commands.Material;
using Items.Api.Dtos;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.Material;

public class MaterialUpdateStatusHandler : IRequestHandler<MaterialUpdateStatusCommand, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialUpdateStatusHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialDto> Handle(MaterialUpdateStatusCommand request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository!.UpdateStatus(request.Id, request.Status, ct);

        _uow.Commit();

        return _mapper.Map<MaterialDto>(result);
    }
}