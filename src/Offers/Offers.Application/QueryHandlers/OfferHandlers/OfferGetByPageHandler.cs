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

        var paginationResult = await _uow.OfferRepository.GetByPage(
            request.Page, 
            request.PageSize, 
            ct, 
            request.OrderBy);

        if (paginationResult is null) return null;

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
