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
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using Microsoft.AspNetCore.Http.Internal;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Icons;

public class IconAddTests : IconTestsSetupBase
{
    private readonly IconAddHandler _sut;
    private readonly IconGetAllHandler _getAll;
    private readonly IconGetByIdHandler _getById;

    private readonly IconAddCommand _command;

    public IconAddTests()
    {
        _sut = new IconAddHandler(_uow, _mapper);
        _getAll = new IconGetAllHandler(_uow, _mapper);
        _getById = new IconGetByIdHandler(_uow, _mapper);
        
        _command = new IconAddCommand
        {
            Title = "new-icon",
            File = new FormFile(new MemoryStream(), 0, 0, "file", "f1"),
        };
    }

    [Fact]
    public async Task ShouldAddNewIconToIconsSet()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new IconGetAllQuery(), CancellationToken.None);
        
        //assert
        all.Should().HaveCount(IconRepositoryMock.InitialFakeDataSet.Count() + 1);
    }


    [Fact]
    public async Task ShouldAddAndReturnIconDto_WithValuesFromCommand() 
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<IconDto>();
        actual.Should().NotBeNull();

        actual!.Title.Should().Be(_command.Title);
        actual.FileName.Should().Be(_command.File?.FileName);
    }

    [Fact]
    public async Task ShouldReturnAddedIconDto() 
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var added = await _getById.Handle(new IconGetByIdQuery(actual.Id), CancellationToken.None);
        
        //assert
        actual.Should().BeOfType<IconDto>();
        actual.Should().Be(added);
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