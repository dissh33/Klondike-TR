using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.Icon;
using Items.Application.QueryHandlers.IconHandlers;
using Items.Domain.Entities;
using Items.Tests.Application.Setups;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Icons;

public class IconGetByIdTests : IconTestsSetup
{
    private readonly IconGetByIdHandler _sut;

    public IconGetByIdTests()
    {
        _sut = new IconGetByIdHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnCorrectIconDto_WhenIconExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new IconGetByIdQuery(id);
        var iconEntity = new Icon("n1", Array.Empty<byte>(), "f1", id);

        _uow.IconRepository.GetById(id, CancellationToken.None).Returns(iconEntity);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(IconDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(iconEntity.Id);
        actual.Title.Should().Be(iconEntity.Title);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenIconDoesNotExists()
    {
        //arrange
        var request = new IconGetByIdQuery(Guid.Empty);

        _uow.IconRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new IconGetByIdQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}