using System;
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

public class CollectionUpdateTests : CollectionTestsSetup
{
    private readonly CollectionUpdateHandler _sut;
    private readonly CollectionGetAllHandler _getAll;
    private readonly CollectionGetByIdHandler _getById;

    private readonly CollectionUpdateCommand _command;

    public CollectionUpdateTests()
    {
        _sut = new CollectionUpdateHandler(_uow, _mapper);
        _getAll = new CollectionGetAllHandler(_uow, _mapper);
        _getById = new CollectionGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new CollectionGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new CollectionUpdateCommand
        {
            Id = fakeId,
            IconId = Guid.NewGuid(),
            Name = "update",
            Status = (int) ItemStatus.Disabled,
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnCollectionDto_WithValuesFromCommand()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        
        //assert
        actual.Should().BeOfType<CollectionDto>();
        actual.Should().NotBeNull();

        actual!.IconId.Should().Be(_command.IconId);
        actual.Status.Should().Be(_command.Status);
        actual.Name.Should().Be(_command.Name);
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
        var doesNotExistsCommand = new CollectionUpdateCommand
        {
            Id = Guid.NewGuid(),
            IconId = Guid.NewGuid(),
            Name = "update",
            Status = (int)ItemStatus.Disabled,
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