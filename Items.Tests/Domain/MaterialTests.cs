using System;
using FluentAssertions;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Xunit;

namespace Items.Tests.Domain;

public class MaterialTests
{
    [Fact]
    public void Material_ShouldConstruct_WithVariousParameters()
    {
        var material = new Material("N1", MaterialType.Specific);
        var material1 = new Material("N2", id: Guid.Empty);
        var material2 = new Material("N3", id: Guid.Empty, externalId: "newID");
        var material3 = new Material("N5", MaterialType.Specific, ItemStatus.Removed, Guid.Empty, "E");

        material.Should().BeOfType(typeof(Material));
        material1.Should().BeOfType(typeof(Material));
        material2.Should().BeOfType(typeof(Material));
        material3.Should().BeOfType(typeof(Material));
    }

    [Fact]
    public void Material_ShouldConstruct_EveryTimeWithNewGuid_WhenIdNotSpecified()
    {
        var collection1 = new Material("N");
        var collection2 = new Material("N");

        collection1.Id.Should().NotBeEmpty();
        collection2.Id.Should().NotBeEmpty();

        collection1.Id.Should().NotBe(collection2.Id);
    }

    [Fact]
    public void Material_ShouldConstruct_WithSpecifiedId_WhenIdPassedThroughConstructor()
    {
        var id = Guid.NewGuid();
        var collection = new Material("N", id: id);
        
        collection.Id.Should().Be(id);
    }

    [Fact]
    public void Material_ShouldConstruct_WithDefaultMaterialTypeAndActiveItemStatus_WhenParametersNotSpecified()
    {
        var material = new Material("N");

        material.Type.Should().HaveFlag(MaterialType.Default);
        material.Status.Should().HaveFlag(ItemStatus.Active);
    }

    [Fact]
    public void Material_ShouldConstruct_WithSpecifiedMaterialTypeAndItemStatus_WhenParametersPassedThroughConstructor()
    {
        var type = MaterialType.Specific;
        var status = ItemStatus.Disabled;
        var material = new Material("N", type:type, status: status);

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