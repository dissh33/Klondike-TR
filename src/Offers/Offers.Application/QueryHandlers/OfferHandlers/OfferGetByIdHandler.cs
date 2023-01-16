using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Queries.Offer;
using Offers.Application.Contracts;

namespace Offers.Application.QueryHandlers.OfferHandlers;

public class OfferGetByIdHandler : IRequestHandler<OfferGetByIdQuery, OfferDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<OfferDto?> Handle(OfferGetByIdQuery request, CancellationToken ct)
    {
        var offer = await _uow.OfferRepository.GetById(request.Id, ct);
        if (offer is null) return null;

        var offerPositions = (await _uow.OfferPositionRepository.GetByOffer(offer.Id.Value, ct)).ToList();

        foreach (var offerPosition in offerPositions)
        {
            offer.AddPosition(
                offerPosition.PriceRate, 
                offerPosition.WithTrader, 
                offerPosition.Message,
                offerPosition.Type, 
                offerPosition.Id.Value);
        }

        foreach (var resultPosition in offer.Positions)
        {
            var offerItems = await _uow.OfferItemRepository.GetByPosition(resultPosition.Id.Value, ct);

            foreach (var offerItem in offerItems)
            {
                resultPosition.AddOfferItem(
                    offerItem.TradableItemId,
                    offerItem.Amount,
                    offerItem.Type,
                    offerItem.Id.Value);
            }
        }

        return _mapper.Map<OfferDto?>(offer);
    }
}
