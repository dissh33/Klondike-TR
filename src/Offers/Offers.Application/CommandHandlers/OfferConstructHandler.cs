﻿using AutoMapper;
using MediatR;
using Offers.Api.Commands;
using Offers.Api.Dtos;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.TypedIds;

namespace Offers.Application.CommandHandlers;

public class OfferConstructHandler : IRequestHandler<OfferConstructCommand, OfferDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferConstructHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

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
            var itemsFromDb = await _uow.OfferItemRepository.BulkInsert(positionFromDto.OfferItems, ct);
            var currentPosition = positionsFromDb.FirstOrDefault(p => p.Id == positionFromDto.Id);

            currentPosition?.AddOfferItems(itemsFromDb);
        }

        offerFromDb.AddPositions(positionsFromDb);

        _uow.Commit();

        return _mapper.Map<OfferDto>(offerFromDb);
    }

    private static Offer ConstructOfferEntity(OfferConstructCommand request)
    {
        var offerStatus = OfferStatus.Draft;
        if (request.Status != null) 
            offerStatus = (OfferStatus)request.Status;

        var offerType = OfferType.Default;
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

            var offerItems = ConstructOfferItems(offerPositionDto, offerPositionId);
            
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

    private static List<OfferItem> ConstructOfferItems(OfferPositionAddDto offerPositionDto, OfferPositionId positionId)
    {
        var offerItems = new List<OfferItem>();

        foreach (var offerItemDto in offerPositionDto.OfferItems)
        {
            var offerItemType = (OfferItemType) offerItemDto.Type;

            var currentItem = new OfferItem(
                positionId,
                offerItemDto.TradableItemId,
                offerItemDto.Amount,
                offerItemType);

            offerItems.Add(currentItem);
        }

        return offerItems;
    }
}