using AutoMapper;
using ItemManagementService.Api.Dtos;
using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.Mapping;

public class MaterialProfile : Profile
{
    public MaterialProfile()
    {
        CreateMap<Material, MaterialDto>();
    }
}