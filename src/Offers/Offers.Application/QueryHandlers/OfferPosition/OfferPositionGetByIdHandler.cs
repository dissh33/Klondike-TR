using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferPosition;
using Offers.Application.Contracts;

namespace Offers.Application.QueryHandlers.OfferPosition;

public class OfferPositionGetByIdHandler : IRequestHandler<OfferPositionGetByIdQuery, OfferPositionDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferPositionGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<OfferPositionDto?> Handle(OfferPositionGetByIdQuery request, CancellationToken ct)
    {
        var offerPosition = await _uow.OfferPositionRepository.GetById(request.Id, ct);
        if (offerPosition is null) return null;

        var offerItems = await _uow.OfferItemRepository.GetByPosition(offerPosition.Id.Value, ct);

        foreach (var offerItem in offerItems)
        {
            offerPosition.AddOfferItem(
                offerItem.TradableItemId,
                offerItem.Amount,
                offerItem.Type,
                offerItem.Id.Value);
        }

        return _mapper.Map<OfferPositionDto?>(offerPosition);
    }
}