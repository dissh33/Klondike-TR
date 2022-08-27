using AutoMapper;
using Items.Api.Commands.Icon;
using Items.Api.Dtos.Icon;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.IconHandlers;

public class IconUpdateFileHandler : IRequestHandler<IconUpdateFileCommand, IconDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IconUpdateFileHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IconDto> Handle(IconUpdateFileCommand request, CancellationToken ct)
    {
        var fileStream = new MemoryStream();
        await request.File!.CopyToAsync(fileStream, ct);
        var binary = fileStream.GetBuffer();
        
        var result = await _uow.IconRepository.UpdateFile(request.Id, request.File.FileName, binary, ct);

        _uow.Commit();

        return _mapper.Map<IconDto>(result);
    }
}
