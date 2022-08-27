using AutoMapper;
using Items.Api.Dtos.Materials;
using Items.Domain.Entities;

namespace Items.Application.Mapping;

public class MaterialProfile : Profile
{
    public MaterialProfile()
    {
        CreateMap<Material, MaterialDto>();
        CreateMap<Material, MaterialFullDto>();
    }
}