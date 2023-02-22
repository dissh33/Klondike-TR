using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Helpers;
using Offers.Api.Queries.Offer;
using Offers.Application.Contracts;

namespace Offers.Application.QueryHandlers.OfferHandlers;

public class OfferGetByPageHandler : IRequestHandler<OfferGetByPageQuery, PaginationWrapper<OfferDto>?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferGetByPageHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginationWrapper<OfferDto>?> Handle(OfferGetByPageQuery request, CancellationToken ct)
    {
        var countResult = await _uow.OfferRepository.GetCount(ct);

        if (countResult == 0) return null;

        var paginationResult = (await _uow.OfferRepository.GetByPage(
            request.Page, 
            request.PageSize, 
            ct, 
            request.OrderBy))?.ToList();

        if (paginationResult is null) return null;

        var positions = (await _uow.OfferPositionRepository.GetByOffers(paginationResult.Select(offer => offer.Id.Value), ct)).ToList();
        var items = (await _uow.OfferItemRepository.GetByPositions(positions.Select(position => position.Id.Value), ct)).ToList();

        foreach (var offer in paginationResult)
        {
            var currentPositions = positions.Where(position => position.OfferId == offer.Id).ToList();

            foreach (var currentPosition in currentPositions)
            {
                var currentItems = items.Where(item => item.OfferPositionId == currentPosition.Id).ToList();
                currentPosition.AddOfferItems(currentItems);
            }

            offer.AddPositions(currentPositions);
        }

        var offerDtos = paginationResult.Select(offer => _mapper.Map<OfferDto>(offer));

        var paginationWrapper = new PaginationWrapper<OfferDto>(offerDtos)
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = countResult,
        };

        return paginationWrapper;
    }
}
