using System;
using AutoMapper;
using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.Mapping;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Tests.Application.Mocks;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using Items.Domain.Entities;
using Items.Domain.Enums;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;
using Items.Tests.Application.Setups;

namespace Items.Tests.Application.Queries.Materials;

public class MaterialGetByIdTests : MaterialTestsSetupBase
{
    private readonly MaterialGetByIdHandler _sut;

    public MaterialGetByIdTests()
    {
        _sut = new MaterialGetByIdHandler(_uow, _mapper);
    }

    [Fact]
    public async Task MaterialGetByIdHandler_ShouldReturnCorrectMaterialDto_WhenMaterialExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new MaterialGetByIdQuery(id);
        var materialResult = new Material("n1", MaterialType.Default, ItemStatus.Disabled, id);

        _uow.MaterialRepository.GetById(id, CancellationToken.None).Returns(materialResult);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(MaterialDto));
        actual.Id.Should().Be(id);
    }

    [Fact]
    public async Task MaterialGetByIdHandler_ShouldReturnNull_WhenMaterialDoesNotExists()
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
    public async Task MaterialGetByIdHandler_ShouldNotCommit()
    {
        //act
        await _sut.Handle(new MaterialGetByIdQuery(Guid.Empty), CancellationToken.None);
        
        //assert
        _uow.Received(0).Commit();
    }
}