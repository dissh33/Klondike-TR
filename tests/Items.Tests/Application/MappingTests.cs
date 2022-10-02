using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AutoMapper;
using Items.Api.Dtos.Collection;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Dtos.Icon;
using Items.Api.Dtos.Materials;
using Items.Application.Mapping;
using Items.Domain.Entities;
using Xunit;

namespace Items.Tests.Application;

public class MappingTests
{
    private readonly IConfigurationProvider _autoMapperConfiguration;
    private readonly IMapper _mapper;

    public MappingTests()
    {

        _autoMapperConfiguration = new MapperConfiguration(config =>
        {
            config.AddProfile<IconProfile>();
            config.AddProfile<MaterialProfile>();
            config.AddProfile<CollectionProfile>();
            config.AddProfile<CollectionItemProfile>();
        });

        _mapper = _autoMapperConfiguration.CreateMapper();
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _autoMapperConfiguration.AssertConfigurationIsValid();
    }

    [Theory]
    [InlineData(typeof(Icon), typeof(IconDto))]
    [InlineData(typeof(Icon), typeof(IconFileDto))]
    [InlineData(typeof(Material), typeof(MaterialDto))]
    [InlineData(typeof(Material), typeof(MaterialFullDto))]
    [InlineData(typeof(Collection), typeof(CollectionDto))]
    [InlineData(typeof(Collection), typeof(CollectionFullDto))]
    [InlineData(typeof(CollectionItem), typeof(CollectionItemDto))]
    [InlineData(typeof(CollectionItem), typeof(CollectionItemFullDto))]
    [InlineData(typeof(CollectionItem), typeof(CollectionItemFullWithFileDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private static object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}