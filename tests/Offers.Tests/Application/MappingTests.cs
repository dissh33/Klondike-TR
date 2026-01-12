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
        }, null);

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

        var ctor = type.GetConstructors()
            .OrderBy(c => c.GetParameters().Length)
            .FirstOrDefault() ?? throw new InvalidOperationException($"No public constructors found for type {type.FullName}.");

        var parameters = ctor.GetParameters()
            .Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null)
            .ToArray();

        return ctor.Invoke(parameters);
    }
}