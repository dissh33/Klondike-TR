using AutoMapper;
using Offers.Api.Dtos;
using Offers.Domain.Entities;

namespace Offers.Application.Mapping;

public class OfferProfile : Profile
{
    public OfferProfile()
    {
        CreateMap<Offer, OfferDto>()
            .ForMember(dto => dto.Id,
                expression => expression.MapFrom(offerPosition => offerPosition.Id.Value));
    }
}