using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands;
using Items.Api.Queries.Collection;
using Items.Application.CommandHandlers.CollectionHandlers;
using Items.Application.QueryHandlers.CollectionHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Collections;

public class CollectionDeleteTests : CollectionTestsSetup
{
    private readonly CollectionDeleteHandler _sut;
    private readonly CollectionGetAllHandler _getAll;

    private readonly DeleteByIdCommand _command;

    public CollectionDeleteTests()
    {
        _sut = new CollectionDeleteHandler(_uow);
        _getAll = new CollectionGetAllHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new CollectionGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;
        _command = new DeleteByIdCommand(fakeId);
    }

    [Fact]
    public async Task ShouldDeleteCollectionFormSetAndReturnZero_WhenCollectionExists()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new CollectionGetAllQuery(), CancellationToken.None);

        actual.Should().Be(0);
        all.Should().HaveCount(CollectionRepositoryMock.InitialFakeDataSet.Count() - 1);
    }

    [Fact]
    public async Task ShouldJustReturnOne_WhenCollectionDoesNotExists()
    {
        //arrange
        var doesNotExistCommand = new DeleteByIdCommand(Guid.NewGuid());

        //act
        var actual = await _sut.Handle(doesNotExistCommand, CancellationToken.None);
        var all = await _getAll.Handle(new CollectionGetAllQuery(), CancellationToken.None);

        //assert
        actual.Should().Be(1);
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