using AutoMapper;
using Offers.Api.Dtos;
using Offers.Domain.Entities;

namespace Offers.Application.Mapping;

public class OfferPositionProfile : Profile
{
    public OfferPositionProfile()
    {
        CreateMap<OfferPosition, OfferPositionDto>()
            .ForMember(dto => dto.Id,
                expression => expression.MapFrom(offerPosition => offerPosition.Id.Value))
            .ForMember(dto => dto.OfferId,
                expression => expression.MapFrom(offerPosition => offerPosition.OfferId != null ? offerPosition.OfferId.Value : Guid.Empty));
    }
}
