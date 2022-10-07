using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.Icon;
using Items.Api.Commands.Material;
using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.CommandHandlers.MaterialHandlers;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Domain.Enums;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Materials;

public class IconUpdateTests : MaterialTestsSetupBase
{
    private readonly MaterialUpdateHandler _sut;
    private readonly MaterialGetAllHandler _getAll;
    private readonly MaterialGetByIdHandler _getById;

    private readonly MaterialUpdateCommand _command;

    public IconUpdateTests()
    {
        _sut = new MaterialUpdateHandler(_uow, _mapper);
        _getAll = new MaterialGetAllHandler(_uow, _mapper);
        _getById = new MaterialGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new MaterialUpdateCommand
        {
            Id = fakeId,
            IconId = Guid.NewGuid(),
            Name = "update",
            Type = (int) MaterialType.Specific,
            Status = (int) ItemStatus.Disabled,
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnMaterialDto_WithValuesFromCommand()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        
        //assert
        actual.Should().BeOfType<MaterialDto>();
        actual.IconId.Should().Be(_command.IconId);
        actual.Type.Should().Be(_command.Type);
        actual.Status.Should().Be(_command.Status);
        actual.Name.Should().Be(_command.Name);
    }

    [Fact]
    public async Task ShouldReturnUpdatedMaterialDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var updated = await _getById.Handle(new MaterialGetByIdQuery(actual.Id), CancellationToken.None);

        //assert
        actual.Should().BeOfType<MaterialDto>();
        actual.Id.Should().Be(updated.Id);
    }

    [Fact]
    public async Task ShouldJustReturnNull_WhenMaterialDoesNotExists()
    {
        //arrange
        var doesNotExistsCommand = new MaterialUpdateCommand
        {
            Id = Guid.NewGuid(),
            IconId = Guid.NewGuid(),
            Name = "update",
            Type = (int)MaterialType.Specific,
            Status = (int)ItemStatus.Disabled,
        };

        //act
        var actual = await _sut.Handle(doesNotExistsCommand, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldUpdateExistedMaterial()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None);

        //assert
        all.Should().HaveCount(MaterialRepositoryMock.InitialFakeDataSet.Count());
    }

    [Fact]
    public async Task ShouldCommitOnce()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);

        //assert
        _uow.Received(1).Commit();
    }
}