﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.Material;
using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.CommandHandlers.MaterialHandlers;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Domain.Enums;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Materials;

public class MaterialUpdateStatusTests : MaterialTestsSetup
{
    private readonly MaterialUpdateStatusHandler _sut;
    private readonly MaterialGetAllHandler _getAll;
    private readonly MaterialGetByIdHandler _getById;

    private readonly MaterialUpdateStatusCommand _command;

    public MaterialUpdateStatusTests()
    {
        _sut = new MaterialUpdateStatusHandler(_uow, _mapper);
        _getAll = new MaterialGetAllHandler(_uow, _mapper);
        _getById = new MaterialGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new MaterialUpdateStatusCommand()
        {
            Id = fakeId,
            Status = (int) ItemStatus.Removed,
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnMaterialDto_WithNewStatusButSameRestValues()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<MaterialDto>();
        actual.Should().NotBeNull();

        actual!.Status.Should().Be(_command.Status);
        actual.Id.Should().Be(_command.Id);

        actual.IconId.Should().Be(MaterialRepositoryMock.InitialFakeDataSet.First().IconId);
        actual.Name.Should().Be(MaterialRepositoryMock.InitialFakeDataSet.First().Name);
        actual.Type.Should().Be((int)MaterialRepositoryMock.InitialFakeDataSet.First().Type);
    }

    [Fact]
    public async Task ShouldReturnUpdatedMaterialDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var updated = await _getById.Handle(new MaterialGetByIdQuery(actual?.Id ?? Guid.Empty), CancellationToken.None);

        //assert
        actual.Should().BeOfType<MaterialDto>();
        actual.Should().NotBeNull();
        updated.Should().NotBeNull();

        actual!.Id.Should().Be(updated!.Id);
    }

    [Fact]
    public async Task ShouldJustReturnNull_WhenMaterialDoesNotExists()
    {
        //arrange
        var doesNotExistsCommand = new MaterialUpdateStatusCommand()
        {
            Id = new Guid(),
            Status = 1,
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