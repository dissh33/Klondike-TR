using MediatR;
using Offers.Api.Commands;
using Offers.Api.Dtos;

namespace Offers.Application.CommandsHandlers;

public class ConstructOfferHandler : IRequestHandler<ConstructOfferCommand, OfferDto>
{
    public Task<OfferDto> Handle(ConstructOfferCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}