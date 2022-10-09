using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.CollectionItem;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.CommandHandlers.CollectionItemHandlers;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.CollectionItems;

public class CollectionItemUpdateTests : CollectionItemTestsSetup
{
    private readonly CollectionItemUpdateHandler _sut;
    private readonly CollectionItemGetAllHandler _getAll;
    private readonly CollectionItemGetByIdHandler _getById;

    private readonly CollectionItemUpdateCommand _command;

    public CollectionItemUpdateTests()
    {
        _sut = new CollectionItemUpdateHandler(_uow, _mapper);
        _getAll = new CollectionItemGetAllHandler(_uow, _mapper);
        _getById = new CollectionItemGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new CollectionItemUpdateCommand
        {
            Id = fakeId,
            CollectionId = Guid.NewGuid(),
            IconId = Guid.NewGuid(),
            Name = "update",
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnCollectionItemDto_WithValuesFromCommand()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        
        //assert
        actual.Should().BeOfType<CollectionItemDto>();
        actual.Should().NotBeNull();

        actual!.IconId.Should().Be(_command.IconId);
        actual.Name.Should().Be(_command.Name);
        actual.CollectionId.Should().Be(_command.CollectionId);
    }

    [Fact]
    public async Task ShouldReturnUpdatedCollectionItemDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var updated = await _getById.Handle(new CollectionItemGetByIdQuery(actual?.Id ?? Guid.Empty), CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionItemDto>();
        actual.Should().NotBeNull();
        updated.Should().NotBeNull();

        actual!.Id.Should().Be(updated!.Id);
    }

    [Fact]
    public async Task ShouldJustReturnNull_WhenCollectionItemDoesNotExists()
    {
        //arrange
        var doesNotExistsCommand = new CollectionItemUpdateCommand
        {
            Id = Guid.NewGuid(),
            CollectionId = Guid.NewGuid(),
            IconId = Guid.NewGuid(),
            Name = "update",
        };

        //act
        var actual = await _sut.Handle(doesNotExistsCommand, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldUpdateExistedCollectionItem()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None);

        //assert
        all.Should().HaveCount(CollectionItemRepositoryMock.InitialFakeDataSet.Count());
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