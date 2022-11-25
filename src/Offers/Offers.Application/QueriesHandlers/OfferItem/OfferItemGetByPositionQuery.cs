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

    public Task<IEnumerable<OfferItemDto>> Handle(OfferItemGetByPositionQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}