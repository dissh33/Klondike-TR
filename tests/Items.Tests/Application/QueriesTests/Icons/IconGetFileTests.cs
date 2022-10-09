using Items.Api.Dtos.Icon;
using Items.Api.Queries.Icon;
using Items.Application.QueryHandlers.IconHandlers;
using Items.Domain.Entities;
using Items.Tests.Application.Setups;
using NSubstitute;
using System.Threading.Tasks;
using System.Threading;
using System;
using FluentAssertions;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Icons;

public class IconGetFileTests : IconTestsSetup
{
    private readonly IconGetFileHandler _sut;

    public IconGetFileTests()
    {
        _sut = new IconGetFileHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnCorrectIconFileDto_WhenIconExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new IconGetFileQuery(id);
        var iconEntity = new Icon("n1", Array.Empty<byte>(), "f1", id);

        _uow.IconRepository.GetFile(id, CancellationToken.None).Returns(iconEntity);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(IconFileDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(iconEntity.Id);
        actual.Title.Should().Be(iconEntity.Title);
        actual.FileName.Should().Be(iconEntity.FileName);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenIconDoesNotExists()
    {
        //arrange
        var request = new IconGetFileQuery(Guid.Empty);

        _uow.IconRepository.GetFile(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new IconGetFileQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}