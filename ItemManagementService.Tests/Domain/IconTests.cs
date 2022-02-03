using System;
using FluentAssertions;
using ItemManagementService.Domain.Entities;
using Xunit;

namespace ItemManagementService.Tests.Domain;

public class IconTests
{
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