using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Items.Api.Dtos;
using Items.Domain.Entities;

namespace Items.Application.Mapping;

public class TradableItemProfile : Profile
{
    public TradableItemProfile()
    {
        CreateMap<Material, TradableItemDto>().ForMember(dto => dto.ItemType, 
            configurationExpression => configurationExpression.MapFrom((material, dto) => dto.ItemType = material.GetType().Name));

        CreateMap<Collection, TradableItemDto>().ForMember(dto => dto.ItemType,
            configurationExpression => configurationExpression.MapFrom((collection, dto) => dto.ItemType = collection.GetType().Name));

        CreateMap<CollectionItem, TradableItemDto>().ForMember(dto => dto.ItemType,
            configurationExpression => configurationExpression.MapFrom((collectionItem, dto) => dto.ItemType = collectionItem.GetType().Name));
    }
}
