using System.Runtime.Serialization;
using AutoMapper;
using Offers.Api.Dtos;
using Offers.Application.Mapping;
using Offers.Domain.Entities;
using Xunit;

namespace Offers.Tests.Application;

public class MappingTests
{
    private readonly IConfigurationProvider _autoMapperConfiguration;
    private readonly IMapper _mapper;

    public MappingTests()
    {

        _autoMapperConfiguration = new MapperConfiguration(config =>
        {
            config.AddProfile<OfferProfile>();
            config.AddProfile<OfferPositionProfile>();
            config.AddProfile<OfferItemProfile>();
        });

        _mapper = _autoMapperConfiguration.CreateMapper();
    }

    [Fact]
    internal void ShouldHaveValidConfiguration()
    {
        _autoMapperConfiguration.AssertConfigurationIsValid();
    }

    [Theory]
    [InlineData(typeof(Offer), typeof(OfferDto))]
    [InlineData(typeof(OfferPosition), typeof(OfferPositionDto))]
    [InlineData(typeof(OfferItem), typeof(OfferItemDto))]
    internal void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
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