using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Domain.Entities;
using Items.Tests.Application.Setups;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.QueriesTests.CollectionItems;

public class CollectionItemGetByIdTests : CollectionItemTestsSetup
{
    private readonly CollectionItemGetByIdHandler _sut;

    public CollectionItemGetByIdTests()
    {
        _sut = new CollectionItemGetByIdHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnCorrectCollectionItemDto_WhenCollectionItemExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new CollectionItemGetByIdQuery(id);
        var collectionItemEntity = new CollectionItem("n1", Guid.NewGuid(), id);

        _uow.CollectionItemRepository.GetById(id, CancellationToken.None).Returns(collectionItemEntity);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(CollectionItemDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(collectionItemEntity.Id);
        actual.Name.Should().Be(collectionItemEntity.Name);
        actual.CollectionId.Should().Be(collectionItemEntity.CollectionId);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenCollectionItemDoesNotExists()
    {
        //arrange
        var request = new CollectionItemGetByIdQuery(Guid.Empty);

        _uow.CollectionItemRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionItemGetByIdQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}