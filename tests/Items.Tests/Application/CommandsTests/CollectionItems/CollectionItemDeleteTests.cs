using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands;
using Items.Api.Queries.CollectionItem;
using Items.Application.CommandHandlers.CollectionItemHandlers;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.CollectionItems;

public class CollectionItemDeleteTests : CollectionItemTestsSetup
{
    private readonly CollectionItemDeleteHandler _sut;
    private readonly CollectionItemGetAllHandler _getAll;

    private readonly DeleteByIdCommand _command;

    public CollectionItemDeleteTests()
    {
        _sut = new CollectionItemDeleteHandler(_uow);
        _getAll = new CollectionItemGetAllHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;
        _command = new DeleteByIdCommand(fakeId);
    }

    [Fact]
    public async Task ShouldDeleteCollectionItemFormSetAndReturnZero_WhenCollectionItemExists()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None);

        actual.Should().Be(0);
        all.Should().HaveCount(CollectionItemRepositoryMock.InitialFakeDataSet.Count() - 1);
    }

    [Fact]
    public async Task ShouldJustReturnOne_WhenCollectionItemDoesNotExists()
    {
        //arrange
        var doesNotExistCommand = new DeleteByIdCommand(Guid.NewGuid());

        //act
        var actual = await _sut.Handle(doesNotExistCommand, CancellationToken.None);
        var all = await _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None);

        //assert
        actual.Should().Be(1);
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