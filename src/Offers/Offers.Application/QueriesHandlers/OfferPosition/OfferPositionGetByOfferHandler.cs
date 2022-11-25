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

    public Task<IEnumerable<OfferPositionDto>> Handle(OfferPositionGetByOfferQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
