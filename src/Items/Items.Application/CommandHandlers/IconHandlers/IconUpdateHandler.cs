using AutoMapper;
using Items.Api.Commands.Icon;
using Items.Api.Dtos.Icon;
using Items.Application.Contracts;
using Items.Domain.Entities;
using MediatR;

namespace Items.Application.CommandHandlers.IconHandlers;

public class IconUpdateHandler : IRequestHandler<IconUpdateCommand, IconDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IconUpdateHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IconDto?> Handle(IconUpdateCommand request, CancellationToken ct)
    {
        var fileStream = new MemoryStream();
        await request.File!.CopyToAsync(fileStream, ct);
        var binary = fileStream.GetBuffer();

        var entity = new Icon(
            request.Title,
            binary,
            request.File.FileName,
            request.Id
        );

        var result = await _uow.IconRepository.Update(entity, ct);

        _uow.Commit();

        return _mapper.Map<IconDto?>(result);
    }
}
