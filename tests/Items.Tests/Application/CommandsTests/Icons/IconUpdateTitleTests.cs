using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.Icon;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.Icon;
using Items.Application.CommandHandlers.IconHandlers;
using Items.Application.QueryHandlers.IconHandlers;
using Items.Domain.Enums;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using Microsoft.AspNetCore.Http.Internal;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Icons;

public class IconUpdateTitleTests : IconTestsSetup
{
    private readonly IconUpdateTitleHandler _sut;
    private readonly IconGetAllHandler _getAll;
    private readonly IconGetByIdHandler _getById;

    private readonly IconUpdateTitleCommand _command;

    public IconUpdateTitleTests()
    {
        _sut = new IconUpdateTitleHandler(_uow, _mapper);
        _getAll = new IconGetAllHandler(_uow, _mapper);
        _getById = new IconGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new IconGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new IconUpdateTitleCommand()
        {
            Id = fakeId,
            Title = "new-title",
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnIconDto_WithNewTitleButSameRestValues()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<IconDto>();
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_command.Id);
        actual.Title.Should().Be(_command.Title);
        actual.FileName.Should().Be(IconRepositoryMock.InitialFakeDataSet.First().FileName);
    }

    [Fact]
    public async Task ShouldReturnUpdatedIconDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var updated = await _getById.Handle(new IconGetByIdQuery(actual?.Id ?? Guid.Empty), CancellationToken.None);

        //assert
        actual.Should().BeOfType<IconDto>();
        actual.Should().NotBeNull();
        updated.Should().NotBeNull();

        actual!.Id.Should().Be(updated!.Id);
    }

    [Fact]
    public async Task ShouldJustReturnNull_WhenIconDoesNotExists()
    {
        //arrange
        var doesNotExistsCommand = new IconUpdateTitleCommand
        {
            Id = new Guid(),
            Title = "update-icon",
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