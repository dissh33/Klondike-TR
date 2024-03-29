﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.Collection;
using Items.Api.Dtos.Collection;
using Items.Api.Queries.Collection;
using Items.Application.CommandHandlers.CollectionHandlers;
using Items.Application.QueryHandlers.CollectionHandlers;
using Items.Domain.Enums;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Collections;

public class CollectionUpdateStatusTests : CollectionTestsSetup
{
    private readonly CollectionUpdateStatusHandler _sut;
    private readonly CollectionGetAllHandler _getAll;
    private readonly CollectionGetByIdHandler _getById;

    private readonly CollectionUpdateStatusCommand _command;

    public CollectionUpdateStatusTests()
    {
        _sut = new CollectionUpdateStatusHandler(_uow, _mapper);
        _getAll = new CollectionGetAllHandler(_uow, _mapper);
        _getById = new CollectionGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new CollectionGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new CollectionUpdateStatusCommand()
        {
            Id = fakeId,
            Status = (int) ItemStatus.Removed,
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnCollectionDto_WithNewStatusButSameRestValues()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionDto>();
        actual.Should().NotBeNull();

        actual!.Status.Should().Be(_command.Status);
        actual.Id.Should().Be(_command.Id);

        actual.IconId.Should().Be(CollectionRepositoryMock.InitialFakeDataSet.First().IconId);
        actual.Name.Should().Be(CollectionRepositoryMock.InitialFakeDataSet.First().Name);
    }

    [Fact]
    public async Task ShouldReturnUpdatedCollectionDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var updated = await _getById.Handle(new CollectionGetByIdQuery(actual?.Id ?? Guid.Empty), CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionDto>();
        actual.Should().NotBeNull();
        updated.Should().NotBeNull();

        actual!.Id.Should().Be(updated!.Id);
    }

    [Fact]
    public async Task ShouldJustReturnNull_WhenCollectionDoesNotExists()
    {
        //arrange
        var doesNotExistsCommand = new CollectionUpdateStatusCommand()
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
    public async Task ShouldUpdateExistedCollection()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new CollectionGetAllQuery(), CancellationToken.None);

        //assert
        all.Should().HaveCount(CollectionRepositoryMock.InitialFakeDataSet.Count());
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