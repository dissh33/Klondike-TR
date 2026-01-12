using AutoMapper;
using MediatR;
using Offers.Api.Commands;
using Offers.Api.Dtos;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.Exceptions;
using Offers.Domain.TypedIds;

namespace Offers.Application.CommandHandlers;

public class OfferConstructHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<OfferConstructCommand, OfferDto?>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;

    public async Task<OfferDto?> Handle(OfferConstructCommand request, CancellationToken ct)
    {
        var offer = ConstructOfferEntity(request);

        var offerFromDb = await _uow.OfferRepository.Insert(offer, ct);

        if (offerFromDb is null)
        {
            _uow.Rollback();
            return null;
        }

        var positionsFromDb = (await _uow.OfferPositionRepository.BulkInsert(offer.Positions, ct)).ToList();
        
        foreach (var positionFromDto in offer.Positions)
        {
            var itemsFromDb = (await _uow.OfferItemRepository.BulkInsert(positionFromDto.OfferItems, ct)).ToList();
            var currentPosition = positionsFromDb.FirstOrDefault(position => position.Id == positionFromDto.Id);

            try
            {
                currentPosition?.AddOfferItems(itemsFromDb);
            }
            catch (MissingOfferItemsException)
            {
                _uow.Rollback();
                throw;
            }
            
        }

        offerFromDb.AddPositions(positionsFromDb);

        _uow.Commit();

        return _mapper.Map<OfferDto>(offerFromDb);
    }
    
    private static Offer ConstructOfferEntity(OfferConstructCommand request)
    {
        var offerStatus = OfferStatus.Draft;
        var offerType = OfferType.Default;

        if (request.Status != null) 
            offerStatus = (OfferStatus)request.Status;

        if (request.Type != null) 
            offerType = (OfferType)request.Type;

        var offer = new Offer(
            request.Title,
            request.Message,
            request.Expression,
            offerType,
            offerStatus);

        var offerPositions = new List<OfferPosition>();

        foreach (var offerPositionDto in request.Positions)
        {
            var offerPositionId = new OfferPositionId(Guid.NewGuid());

            var offerItems = offerPositionDto.OfferItems.Select(offerItemDto => 
                new OfferItem(
                    offerPositionId, 
                    offerItemDto.TradableItemId, 
                    offerItemDto.Amount, 
                    (OfferItemType) offerItemDto.Type))
                .ToList();

            var currentPosition = new OfferPosition(
                offer.Id,
                offerPositionDto.PriceRate,
                offerPositionDto.WithTrader,
                offerPositionDto.Message,
                (OfferPositionType)offerPositionDto.Type,
                offerPositionId.Value,
                offerItems);
            
            offerPositions.Add(currentPosition);
        }

        offer.AddPositions(offerPositions);

        return offer;
    }
}