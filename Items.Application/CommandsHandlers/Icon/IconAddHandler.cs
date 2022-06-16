using AutoMapper;
using Items.Api.Commands.Icon;
using Items.Api.Dtos;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.Icon;

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
        var fileStream = new MemoryStream();
        await request.File!.CopyToAsync(fileStream, ct);
        var binary = fileStream.GetBuffer();

        var entity = new Domain.Entities.Icon(
            request.Title,
            binary,
            request.File.FileName
        );

       var result = await _uow.IconRepository!.Insert(entity, ct);

       _uow.Commit();

       return _mapper.Map<IconDto>(result);
    }
}
