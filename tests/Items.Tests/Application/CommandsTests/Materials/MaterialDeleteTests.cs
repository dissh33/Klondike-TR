using System;
using System.Linq;
using Items.Application.CommandHandlers.MaterialHandlers;
using NSubstitute;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using Items.Api.Commands;
using Xunit;
using Items.Api.Queries.Material;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;

namespace Items.Tests.Application.CommandsTests.Materials;

public class MaterialDeleteTests : MaterialTestsSetup
{
    private readonly MaterialDeleteHandler _sut;
    private readonly MaterialGetAllHandler _getAll;

    private readonly DeleteByIdCommand _command;

    public MaterialDeleteTests()
    {
        _sut = new MaterialDeleteHandler(_uow);
        _getAll = new MaterialGetAllHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;
        _command = new DeleteByIdCommand(fakeId);
    }

    [Fact]
    public async Task ShouldDeleteMaterialFormMaterialsSetAndReturnZero_WhenMaterialExists()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None);

        actual.Should().Be(0);
        all.Should().HaveCount(MaterialRepositoryMock.InitialFakeDataSet.Count() - 1);
    }

    [Fact]
    public async Task ShouldJustReturnOne_WhenMaterialDoesNotExists()
    {
        //arrange
        var doesNotExistCommand = new DeleteByIdCommand(Guid.NewGuid());

        //act
        var actual = await _sut.Handle(doesNotExistCommand, CancellationToken.None);
        var all = await _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None);

        //assert
        actual.Should().Be(1);
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