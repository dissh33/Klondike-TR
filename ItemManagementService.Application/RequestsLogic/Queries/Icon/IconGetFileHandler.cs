using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Api.Queries.Icon;
using ItemManagementService.Application.Contracts;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Queries.Icon;

public class IconGetFileHandler : IRequestHandler<IconGetFileQuery, IconFileDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IconGetFileHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IconFileDto> Handle(IconGetFileQuery request, CancellationToken ct)
    {
        var result = await _uow.IconRepository!.GetFile(request.Id, ct);
        
        var dto = _mapper.Map<IconFileDto>(result);

        return dto;
    }
}
