using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferPosition;
using Offers.Application.Contracts;

namespace Offers.Application.QueriesHandlers.OfferPosition;

public class OfferPositionGetByOfferHandler : IRequestHandler<OfferPositionGetByOfferQuery, IEnumerable<OfferPositionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferPositionGetByOfferHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OfferPositionDto>> Handle(OfferPositionGetByOfferQuery request, CancellationToken ct)
    {
        var offerPositions = (await _uow.OfferPositionRepository.GetByOffer(request.OfferId, ct)).ToList();

        foreach (var offerPosition in offerPositions)
        {
            var offerItems = await _uow.OfferItemRepository.GetByPosition(offerPosition.Id.Value, ct);

            foreach (var offerItem in offerItems)
            {
                offerPosition.AddOfferItem(
                    offerItem.TradableItemId,
                    offerItem.Amount,
                    offerItem.Type,
                    offerItem.Id.Value);
            }
        }

        return offerPositions.Select(position => _mapper.Map<OfferPositionDto>(position));
    }
}
