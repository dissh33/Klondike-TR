using System.IO;
using System.Linq;
using Items.Api.Commands.Icon;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.Icon;
using Items.Application.CommandHandlers.IconHandlers;
using Items.Application.QueryHandlers.IconHandlers;
using Items.Tests.Application.Mocks;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using Items.Tests.Application.Setups;
using Microsoft.AspNetCore.Http.Internal;
using NSubstitute;
using Xunit;
using System;

namespace Items.Tests.Application.CommandsTests.Icons;

public class IconUpdateFileTests : IconTestsSetupBase
{
    private readonly IconUpdateFileHandler _sut;
    private readonly IconGetAllHandler _getAll;
    private readonly IconGetByIdHandler _getById;

    private readonly IconUpdateFileCommand _command;

    public IconUpdateFileTests()
    {
        _sut = new IconUpdateFileHandler(_uow, _mapper);
        _getAll = new IconGetAllHandler(_uow, _mapper);
        _getById = new IconGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new IconGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new IconUpdateFileCommand()
        {
            Id = fakeId,
            File = new FormFile(new MemoryStream(), 0, 0, "file", "new-file-name"),
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnIconDto_WithNewFileButSameRestValues()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<IconDto>();
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_command.Id);
        actual.FileName.Should().Be(_command.File?.FileName);
        actual.Title.Should().Be(IconRepositoryMock.InitialFakeDataSet.First().Title);
    }

    [Fact]
    public async Task ShouldReturnUpdatedIconDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var updated = await _getById.Handle(new IconGetByIdQuery(actual.Id), CancellationToken.None);

        //assert
        actual.Should().BeOfType<IconDto>();
        actual.Should().NotBeNull();
        updated.Should().NotBeNull();

        actual.Id.Should().Be(updated!.Id);
    }

    [Fact]
    public async Task ShouldJustReturnNull_WhenIconDoesNotExists()
    {
        //arrange
        var doesNotExistsCommand = new IconUpdateFileCommand
        {
            Id = new Guid(),
            File = new FormFile(new MemoryStream(), 0, 0, "file", "f1"),
        };

        //act
        var actual = await _sut.Handle(doesNotExistsCommand, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldUpdateExistedIcon()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new IconGetAllQuery(), CancellationToken.None);

        //assert
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