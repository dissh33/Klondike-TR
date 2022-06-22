using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.Material;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.MaterialHandlers;

public class MaterialGetByIdHandler : IRequestHandler<MaterialGetByIdQuery, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaterialGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<MaterialDto> Handle(MaterialGetByIdQuery request, CancellationToken ct)
    {
        var result = await _uow.MaterialRepository!.GetById(request.Id, ct);

        return _mapper.Map<MaterialDto>(result);
    }
}