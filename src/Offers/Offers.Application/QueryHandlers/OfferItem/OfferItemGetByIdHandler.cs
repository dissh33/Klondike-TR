using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferItem;
using Offers.Application.Contracts;

namespace Offers.Application.QueryHandlers.OfferItem;

public class OfferItemGetByIdHandler : IRequestHandler<OfferItemGetByIdQuery, OfferItemDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferItemGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<OfferItemDto?> Handle(OfferItemGetByIdQuery request, CancellationToken ct)
    {
        var result = await _uow.OfferItemRepository.GetById(request.Id, ct);

        return _mapper.Map<OfferItemDto?>(result);
    }
}