using AutoMapper;
using Items.Api.Commands.Icon;
using Items.Api.Dtos;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Commands.Icon;

public class IconUpdateTitleHandler : IRequestHandler<IconUpdateTitleCommand, IconDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IconUpdateTitleHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IconDto> Handle(IconUpdateTitleCommand request, CancellationToken ct)
    {
        var result = await _uow.IconRepository!.UpdateTitle(request.Id, request.Title, ct);

        _uow.Commit();

        return _mapper.Map<IconDto>(result);
    }
}
