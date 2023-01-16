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
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.CollectionItems;

public class CollectionItemUpdateCollectionTests : CollectionItemTestsSetup
{
    private readonly CollectionItemUpdateCollectionHandler _sut;
    private readonly CollectionItemGetAllHandler _getAll;
    private readonly CollectionItemGetByIdHandler _getById;

    private readonly CollectionItemUpdateCollectionCommand _command;

    public CollectionItemUpdateCollectionTests()
    {
        _sut = new CollectionItemUpdateCollectionHandler(_uow, _mapper);
        _getAll = new CollectionItemGetAllHandler(_uow, _mapper);
        _getById = new CollectionItemGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new CollectionItemUpdateCollectionCommand()
        {
            Id = fakeId,
            CollectionId = Guid.NewGuid(),
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnCollectionItemDto_WithNewCollectionButSameRestValues()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionItemDto>();
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_command.Id);
        actual!.CollectionId.Should().Be(_command.CollectionId);
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
        var doesNotExistsCommand = new CollectionItemUpdateCollectionCommand
        {
            Id = new Guid(),
            CollectionId = Guid.NewGuid(),
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