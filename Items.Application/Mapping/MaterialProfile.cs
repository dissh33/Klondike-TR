using AutoMapper;
using Items.Api.Dtos;
using Items.Domain.Entities;

namespace Items.Application.Mapping;

public class MaterialProfile : Profile
{
    public MaterialProfile()
    {
        CreateMap<Material, MaterialDto>();
    }
}