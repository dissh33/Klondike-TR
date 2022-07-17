using MediatR;
using Offers.Api.Dtos;

namespace Offers.Api.Commands;

public class ConstructOfferCommand : IRequest<OfferDto>
{
}