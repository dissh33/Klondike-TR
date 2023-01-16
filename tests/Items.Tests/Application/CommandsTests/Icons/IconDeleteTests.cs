using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands;
using Items.Api.Queries.Icon;
using Items.Application.CommandHandlers.IconHandlers;
using Items.Application.QueryHandlers.IconHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Icons;

public class IconDeleteTests : IconTestsSetup
{
    private readonly IconDeleteHandler _sut;
    private readonly IconGetAllHandler _getAll;

    private readonly DeleteByIdCommand _command;

    public IconDeleteTests()
    {
        _sut = new IconDeleteHandler(_uow);
        _getAll = new IconGetAllHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new IconGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;
        _command = new DeleteByIdCommand(fakeId);
    }

    [Fact]
    public async Task ShouldDeleteIconFormSetAndReturnZero_WhenIconExists()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new IconGetAllQuery(), CancellationToken.None);

        actual.Should().Be(0);
        all.Should().HaveCount(IconRepositoryMock.InitialFakeDataSet.Count() - 1);
    }

    [Fact]
    public async Task ShouldJustReturnOne_WhenIconDoesNotExists()
    {
        //arrange
        var doesNotExistCommand = new DeleteByIdCommand(Guid.NewGuid());

        //act
        var actual = await _sut.Handle(doesNotExistCommand, CancellationToken.None);
        var all = await _getAll.Handle(new IconGetAllQuery(), CancellationToken.None);

        //assert
        actual.Should().Be(1);
        all.Should().HaveCount(IconRepositoryMock.InitialFakeDataSet.Count());
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