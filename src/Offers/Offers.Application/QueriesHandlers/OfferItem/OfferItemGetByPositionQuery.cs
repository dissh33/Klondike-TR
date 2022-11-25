using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferItem;
using Offers.Application.Contracts;

namespace Offers.Application.QueriesHandlers.OfferItem;

public class OfferItemGetByPositionHandler : IRequestHandler<OfferItemGetByPositionQuery, IEnumerable<OfferItemDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferItemGetByPositionHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OfferItemDto>> Handle(OfferItemGetByPositionQuery request, CancellationToken ct)
    {
        var result = await _uow.OfferItemRepository.GetByPosition(request.PositionId, ct);

        return result.Select(offerItem => _mapper.Map<OfferItemDto>(offerItem));
    }
}