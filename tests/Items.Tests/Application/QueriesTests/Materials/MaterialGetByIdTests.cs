using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Tests.Application.Setups;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Materials;

public class MaterialGetByIdTests : MaterialTestsSetupBase
{
    private readonly MaterialGetByIdHandler _sut;

    public MaterialGetByIdTests()
    {
        _sut = new MaterialGetByIdHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnCorrectMaterialDto_WhenMaterialExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new MaterialGetByIdQuery(id);
        var materialEntity = new Material("n1", MaterialType.Default, ItemStatus.Disabled, id);

        _uow.MaterialRepository.GetById(id, CancellationToken.None).Returns(materialEntity);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(MaterialDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(materialEntity.Id);
        actual.Name.Should().Be(materialEntity.Name);
        actual.Type.Should().Be((int) materialEntity.Type);
        actual.Status.Should().Be((int) materialEntity.Status);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenMaterialDoesNotExists()
    {
        //arrange
        var request = new MaterialGetByIdQuery(Guid.Empty);

        _uow.MaterialRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new MaterialGetByIdQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}