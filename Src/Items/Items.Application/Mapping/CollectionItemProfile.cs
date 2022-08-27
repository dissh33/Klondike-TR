using AutoMapper;
using Items.Api.Dtos.CollectionItem;
using Items.Domain.Entities;

namespace Items.Application.Mapping;

public class CollectionItemProfile : Profile
{
    public CollectionItemProfile()
    {
        CreateMap<CollectionItem, CollectionItemDto>();
        CreateMap<CollectionItem, CollectionItemFullDto>();
        CreateMap<CollectionItem, CollectionItemFullWithFileDto>();
    }
}