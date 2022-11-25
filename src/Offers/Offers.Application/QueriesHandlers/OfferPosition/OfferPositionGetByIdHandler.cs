using AutoMapper;
using MediatR;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferPosition;
using Offers.Application.Contracts;

namespace Offers.Application.QueriesHandlers.OfferPosition;

public class OfferPositionGetByIdHandler : IRequestHandler<OfferPositionGetByIdQuery, OfferPositionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public OfferPositionGetByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<OfferPositionDto> Handle(OfferPositionGetByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}