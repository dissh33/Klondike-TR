﻿using AutoMapper;
using MediatR;
using Offers.Api.Commands;
using Offers.Api.Dtos;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;

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
        var offer = ConstructOfferForInsert(request);

        var offerResult = await _uow.OfferRepository.Insert(offer, ct);

        if (offerResult is null)
        {
            _uow.Rollback();
            return null;
        }

        var positionsResult = await _uow.OfferPositionRepository.BulkInsert(offer.Positions, ct);
        
        foreach (var position in positionsResult)
        {
            var itemsResult = await _uow.OfferItemRepository.BulkInsert(position.OfferItems, ct);

            foreach (var item in itemsResult)
            {
                position.AddOfferItem(
                    item.TradableItemId, 
                    item.Amount, 
                    item.Type, 
                    item.Id.Value);
            }

            offerResult.AddPosition(
                position.PriceRate,
                position.WithTrader,
                position.Message,
                position.Type,
                position.Id.Value);
        }

        _uow.Commit();

        return _mapper.Map<OfferDto>(offerResult);
    }

    private static Offer ConstructOfferForInsert(OfferConstructCommand request)
    {
        var status = OfferStatus.Draft;
        if (request.Status != null) status = (OfferStatus) request.Status;

        var offerType = OfferType.Default;
        if (request.Type != null) offerType = (OfferType) request.Type;

        var offer = new Offer(
            request.Title,
            request.Message,
            request.Expression,
            offerType,
            status);

        foreach (var offerPositionDto in request.Positions)
        {
            var offerPositionType = (OfferPositionType) offerPositionDto.Type;

            var offerPositionId = offer.AddPosition(
                offerPositionDto.PriceRate,
                offerPositionDto.WithTrader,
                offerPositionDto.Message,
                offerPositionType);

            var currentPosition = offer.Positions.First(position => position.Id == offerPositionId);

            foreach (var offerItemDto in offerPositionDto.OfferItems)
            {
                var offerItemType = (OfferItemType) offerItemDto.Type;

                currentPosition.AddOfferItem(
                    offerItemDto.TradableItemId,
                    offerItemDto.Amount,
                    offerItemType);
            }
        }

        return offer;
    }
}