using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.Mapping;

public class CollectionProfile : Profile
{
    public CollectionProfile()
    {
        CreateMap<Collection, CollectionDto>();
    }
}