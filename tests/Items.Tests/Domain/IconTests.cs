using System;
using System.Collections.Generic;
using FluentAssertions;
using Items.Domain.Entities;
using Xunit;

namespace Items.Tests.Domain;

public class IconTests
{
    [Theory]
    [MemberData(nameof(IconConstructorParameters))]
    public void Icon_ShouldConstruct_WithVariousParameters(
        string title,
        byte[] fileBinary,
        string fileName,
        Guid? id,
        string? externalId)
    {
        var icon = new Icon(title, fileBinary, fileName, id, externalId);

        icon.Should().BeOfType(typeof(Icon));
    }

    private static IEnumerable<object?[]> IconConstructorParameters()
    {
        return new List<object?[]>
        {
            new object?[] { "t1", Array.Empty<byte>(), "f1", Guid.Empty, "eID" },
            new object?[] { "t2", null, null, null, "eID" },
            new object?[] { null , null, null, Guid.Empty, "eID" },
        };
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
    public void Icon_ShouldConstruct_WithSpecifiedId_WhenIdPassed()
    {
        var id = Guid.NewGuid();
        var icon = new Icon("T", Array.Empty<byte>(), "F", id);
        var icon1 = new Icon(id);
        
        icon.Id.Should().Be(id);
        icon1.Id.Should().Be(id);

        icon1.Id.Should().NotBe(id);
    }
}