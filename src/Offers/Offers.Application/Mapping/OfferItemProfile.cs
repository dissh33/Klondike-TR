using AutoMapper;
using Offers.Api.Dtos;
using Offers.Domain.Entities;

namespace Offers.Application.Mapping;

public class OfferItemProfile : Profile
{
    public OfferItemProfile()
    {
        CreateMap<OfferItem, OfferItemDto>();
    }
}
