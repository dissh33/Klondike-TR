using System;
using FluentAssertions;
using Items.Domain.Entities;
using Xunit;

namespace Items.Tests.Domain;

public class IconTests
{
    [Fact]
    public void Icon_ShouldConstruct_WithVariousParameters()
    {
        var id = Guid.NewGuid();
        var icon1 = new Icon("T", Array.Empty<byte>(), "F", id);
        var icon2 = new Icon("T2", Array.Empty<byte>(), "F2", externalId: "newid");
        var icon3 = new Icon();

        icon1.Should().BeOfType(typeof(Icon));
        icon2.Should().BeOfType(typeof(Icon));
        icon3.Should().BeOfType(typeof(Icon));
    }

    [Fact]
    public void Icon_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var icon1 = new Icon("T1", Array.Empty<byte>(), "F1");
        var icon2 = new Icon("T2", Array.Empty<byte>(), "F2");

        icon1.Id.Should().NotBeEmpty();
        icon2.Id.Should().NotBeEmpty();

        icon1.Id.Should().NotBe(icon2.Id);
    }

    [Fact]
    public void Icon_ShouldConstruct_WithSpecifiedId_WhenIdPassedThroughConstructor()
    {
        var id = Guid.NewGuid();
        var icon = new Icon("T", Array.Empty<byte>(), "F", id);
        
        icon.Id.Should().NotBeEmpty();
        icon.Id.Should().Be(id);
    }
}