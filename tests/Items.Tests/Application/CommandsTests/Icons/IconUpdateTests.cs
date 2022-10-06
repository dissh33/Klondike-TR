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

public class IconUpdateTests : IconTestsSetupBase
{
    private readonly IconUpdateHandler _sut;
    private readonly IconGetAllHandler _getAll;
    private readonly IconGetByIdHandler _getById;

    private readonly IconUpdateCommand _command;

    public IconUpdateTests()
    {
        _sut = new IconUpdateHandler(_uow, _mapper);
        _getAll = new IconGetAllHandler(_uow, _mapper);
        _getById = new IconGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new IconGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new IconUpdateCommand
        {
            Id = fakeId,
            Title = "update-icon",
            File = new FormFile(new MemoryStream(), 0, 0, "file", "f1"),
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnIconDto_WithValuesFromCommand()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        
        //assert
        actual.Should().BeOfType<IconDto>();
        actual.Id.Should().Be(_command.Id);
        actual.Title.Should().Be(_command.Title);
        actual.FileName.Should().Be(_command.File?.FileName);
    }

    [Fact]
    public async Task ShouldReturnUpdatedIconDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var updated = await _getById.Handle(new IconGetByIdQuery(actual.Id), CancellationToken.None);

        //assert
        actual.Should().BeOfType<IconDto>();
        actual.Id.Should().Be(updated.Id);
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