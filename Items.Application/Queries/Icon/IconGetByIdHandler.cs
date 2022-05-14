using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.Icon;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.Queries.Icon;

public class IconGetByIdHandler : IRequestHandler<IconGetByIdQuery, IconDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IconGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IconDto> Handle(IconGetByIdQuery request, CancellationToken ct)
    {
        var result = await _uow.IconRepository!.GetById(request.Id, ct);

        return _mapper.Map<IconDto>(result);
    }
}