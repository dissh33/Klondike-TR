using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Collection;
using Items.Api.Queries.Collection;
using Items.Application.QueryHandlers.CollectionHandlers;
using Items.Tests.Application.Setup;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Collections;

public class CollectionGetByIdTests : CollectionTestsSetup
{
    private readonly CollectionGetByIdHandler _sut;

    public CollectionGetByIdTests()
    {
        _sut = new CollectionGetByIdHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnCorrectCollectionDto_WhenCollectionExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new CollectionGetByIdQuery(id);
        var collectionEntity = new Items.Domain.Entities.Collection("n1");

        _uow.CollectionRepository.GetById(id, CancellationToken.None).Returns(collectionEntity);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(CollectionDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(collectionEntity.Id);
        actual.Name.Should().Be(collectionEntity.Name);
        actual.Status.Should().Be((int) collectionEntity.Status!);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenCollectionDoesNotExists()
    {
        //arrange
        var request = new CollectionGetByIdQuery(Guid.Empty);

        _uow.CollectionRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionGetByIdQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}