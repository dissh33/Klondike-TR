using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Tests.Application.Setups;
using NSubstitute;
using System.Threading.Tasks;
using System.Threading;
using System;
using FluentAssertions;
using Items.Api.Dtos.Icon;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Materials;

public class MaterialGetFullTests : MaterialTestsSetup
{
    private readonly MaterialGetFullHandler _sut;

    private readonly Guid _materialId;
    private readonly Material _materialEntity;
    private readonly Icon _iconEntity;

    public MaterialGetFullTests()
    {
        _sut = new MaterialGetFullHandler(_uow, _mapper);
        
        _materialId = Guid.NewGuid();
        _materialEntity = new Material("n1", MaterialType.Default, ItemStatus.Disabled, _materialId);
        _iconEntity = new Icon("n1", Array.Empty<byte>(), "f1");

        _uow.IconRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).Returns(_iconEntity);
        _uow.MaterialRepository.GetById(_materialId, CancellationToken.None).Returns(_materialEntity);
    }

    [Fact]
    public async Task ShouldReturnCorrectMaterialFullDto_WhenMaterialAndIconExist()
    {
        //act
        var actual = await _sut.Handle(new MaterialGetFullQuery(_materialId), CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(MaterialFullDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_materialEntity.Id);
        actual.Name.Should().Be(_materialEntity.Name);
        actual.Type.Should().Be((int)_materialEntity.Type);
        actual.Status.Should().Be((int)_materialEntity.Status);

        actual.Icon.Should().BeOfType(typeof(IconDto));
        actual.Icon.Should().NotBeNull();
        actual.Icon!.Id.Should().Be(_iconEntity.Id);
        actual.Icon.Title.Should().Be(_iconEntity.Title);
        actual.Icon.FileName.Should().Be(_iconEntity.FileName);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenMaterialDoesNotExists()
    {
        //arrange
        var request = new MaterialGetFullQuery(Guid.Empty);

        _uow.MaterialRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldReturnMaterialFullDtoWithEmptyIcon_WhenMaterialExists_ButIconDoesNotExists()
    {
        //arrange
        var request = new MaterialGetFullQuery(_materialId);

        _uow.IconRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(MaterialFullDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_materialEntity.Id);
        actual.Name.Should().Be(_materialEntity.Name);
        actual.Type.Should().Be((int)_materialEntity.Type);
        actual.Status.Should().Be((int)_materialEntity.Status);

        actual.Icon.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new MaterialGetFullQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}