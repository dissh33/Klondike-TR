using AutoMapper;
using Items.Api.Dtos;
using Items.Api.Queries.Icon;
using Items.Application.Contracts;
using MediatR;

namespace Items.Application.Queries.IconHandlers;

public class IconGetAllHandler : IRequestHandler<IconGetAllQuery, IEnumerable<IconDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IconGetAllHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<IconDto>> Handle(IconGetAllQuery request, CancellationToken ct)
    {
        var result = await _uow.IconRepository.GetAll(ct);

        return result.Select(icon => _mapper.Map<IconDto>(icon));
    }
}