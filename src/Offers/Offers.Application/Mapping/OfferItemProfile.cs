using AutoMapper;
using Offers.Api.Dtos;
using Offers.Domain.Entities;

namespace Offers.Application.Mapping;

public class OfferItemProfile : Profile
{
    public OfferItemProfile()
    {
        CreateMap<OfferItem, OfferItemDto>()
            .ForMember(dto => dto.Id,
                expression => expression.MapFrom(offerPosition => offerPosition.Id.Value))
            .ForMember(dto => dto.OfferPositionId,
                expression => expression.MapFrom(offerPosition => offerPosition.OfferPositionId != null ? offerPosition.OfferPositionId.Value : Guid.Empty));
    }
}
