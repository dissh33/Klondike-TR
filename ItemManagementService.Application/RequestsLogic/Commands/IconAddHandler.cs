using AutoMapper;
using ItemManagementService.Api.Commands;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using MediatR;

namespace ItemManagementService.Application.RequestsLogic.Commands;

public class IconAddHandler : IRequestHandler<IconAddCommand, IconDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IconAddHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IconDto> Handle(IconAddCommand request, CancellationToken ct)
    {
        var entity = new Icon(
            request.Title,
            request.FileBinary,
            request.FileName
        );

       var result = await _uow.IconRepository!.Insert(entity, ct);

       return _mapper.Map<IconDto>(result);
    }
}
