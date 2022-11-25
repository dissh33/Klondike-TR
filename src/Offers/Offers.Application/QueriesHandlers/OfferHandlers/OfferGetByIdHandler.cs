using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Queries.Offer;
using Offers.Application.Contracts;

namespace Offers.Application.QueriesHandlers.OfferHandlers;

public class OfferItemGetByIdHandler : IRequestHandler<OfferGetByIdQuery, OfferDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferItemGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<OfferDto> Handle(OfferGetByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
