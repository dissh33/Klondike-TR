using AutoMapper;
using Offers.Api.Dtos;
using Offers.Domain.Entities;

namespace Offers.Application.Mapping;

public class OfferPositionProfile : Profile
{
    public OfferPositionProfile()
    {
        CreateMap<OfferPosition, OfferPositionDto>();
    }
}
