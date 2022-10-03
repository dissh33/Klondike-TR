using System;
using System.Collections.Generic;
using System.Security;
using FluentAssertions;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Xunit;

namespace Items.Tests.Domain;

public class MaterialTests
{
    [Theory]
    [MemberData(nameof(MaterialConstructorParameters))]
    public void Material_ShouldConstruct_WithVariousParameters(
        string? name,
        MaterialType type,
        ItemStatus status,
        Guid? id,
        string? externalId)
    {
        var material = new Material(name, type, status, id, externalId);

        material.Should().BeOfType(typeof(Material));
    }

    private static IEnumerable<object?[]> MaterialConstructorParameters()
    {
        return new List<object?[]>
        {
            new object?[] { "n1", default, default, null, null },
            new object?[] { "n2", MaterialType.Specific, default, null, null },
            new object?[] { "n3", MaterialType.Specific, ItemStatus.Disabled, null, null },
            new object?[] { "n4", default, default, Guid.Empty, null },
            new object?[] { "n5", MaterialType.Specific, ItemStatus.Disabled, Guid.Empty, "eID" },
        };
    }

    [Fact]
    public void Material_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var material1 = new Material("N");
        var material2 = new Material("N");

        material1.Id.Should().NotBeEmpty();
        material2.Id.Should().NotBeEmpty();

        material1.Id.Should().NotBe(material2.Id);
    }

    [Fact]
    public void Material_ShouldConstruct_WithSpecifiedId_WhenIdPassed()
    {
        var id = Guid.NewGuid();
        var material = new Material("N", id: id);
        
        material.Id.Should().Be(id);
    }

    [Fact]
    public void Material_ShouldConstruct_WithDefaultMaterialTypeAndActiveItemStatus_WhenParametersNotSpecified()
    {
        var material = new Material("N");

        material.Type.Should().HaveFlag(MaterialType.Default);
        material.Status.Should().HaveFlag(ItemStatus.Available);
    }

    [Fact]
    public void Material_ShouldConstruct_WithSpecifiedMaterialTypeAndItemStatus_WhenTypeAndStatusPassed()
    {
        var type = MaterialType.Specific;
        var status = ItemStatus.Disabled;
        var material = new Material("N", type: type, status: status);

        material.Type.Should().HaveFlag(type);
        material.Status.Should().HaveFlag(status);
    }

    [Fact]
    public void AddIconMethod_ShouldCorrectlyAddIconToMaterial_WhenPassedFullIconInfo()
    {
        var material = new Material("N");

        material.AddIcon("T", new byte[1], "N");
        
        material.Icon.Should().BeOfType<Icon>();
        material.IconId.Should().Be(material.Icon!.Id);
    }

    [Fact]
    public void AddIconMethod_ShouldAddIconWithSpecifiedIdToMaterial_WhenPassedOnlyIconId()
    {
        var material = new Material("N");
        var iconId = Guid.NewGuid();

        material.AddIcon(iconId);
        
        material.Icon.Should().BeOfType<Icon>();
        
        material.IconId.Should().Be(iconId);
        material.IconId.Should().Be(material.Icon!.Id);
    }
}