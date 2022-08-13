using AutoMapper;
using Items.Api.Dtos;
using Items.Domain.Entities;

namespace Items.Application.Mapping;

public class IconProfile : Profile
{
    public IconProfile()
    {
        CreateMap<Icon, IconDto>();
        CreateMap<Icon, IconFileDto>();
    }
}