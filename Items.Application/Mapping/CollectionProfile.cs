using AutoMapper;
using Items.Api.Dtos;
using Items.Domain.Entities;

namespace Items.Application.Mapping;

public class CollectionProfile : Profile
{
    public CollectionProfile()
    {
        CreateMap<Collection, CollectionDto>();
        CreateMap<Collection, CollectionFullDto>();
    }
}