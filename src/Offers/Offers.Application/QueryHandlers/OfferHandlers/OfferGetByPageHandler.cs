using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Queries.Offer;
using Offers.Application.Contracts;

namespace Offers.Application.QueryHandlers.OfferHandlers;

public class OfferGetByPageHandler : IRequestHandler<OfferGetByPageQuery, IEnumerable<OfferDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferGetByPageHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OfferDto>> Handle(OfferGetByPageQuery request, CancellationToken cancellationToken)
    {
        return new List<OfferDto>();
    }
}
