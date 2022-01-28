using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.RequestsLogic.Mapping;

public class IconProfile : Profile
{
    public IconProfile()
    {
        CreateMap<Icon, IconDto>();
        CreateMap<Icon, IconFileDto>();
    }
}