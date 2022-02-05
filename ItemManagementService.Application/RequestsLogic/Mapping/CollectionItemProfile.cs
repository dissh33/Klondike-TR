using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.RequestsLogic.Mapping;

public class CollectionItemProfile : Profile
{
    public CollectionItemProfile()
    {
        CreateMap<CollectionItem, CollectionItemDto>();
    }
}