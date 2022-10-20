using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.CollectionItem;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Domain.Entities;
using Items.Tests.Application.Setups;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.QueriesTests.CollectionItems;

public class CollectionItemGetFullTests : CollectionItemTestsSetup
{
    private readonly CollectionItemGetFullHandler _sut;

    private readonly Guid _collectionItemId;
    private readonly CollectionItem _collectionItemEntity;
    private readonly Icon _iconEntity;

    public CollectionItemGetFullTests()
    {
        _sut = new CollectionItemGetFullHandler(_uow, _mapper);
        
        _collectionItemId = Guid.NewGuid();
        _collectionItemEntity = new CollectionItem("n1", Guid.NewGuid(), _collectionItemId);
        _iconEntity = new Icon("n1", Array.Empty<byte>(), "f1");

        _uow.IconRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).Returns(_iconEntity);
        _uow.CollectionItemRepository.GetById(_collectionItemId, CancellationToken.None).Returns(_collectionItemEntity);
    }

    [Fact]
    public async Task ShouldReturnCorrectCollectionItemFullDto_WhenCollectionItemAndIconExist()
    {
        //act
        var actual = await _sut.Handle(new CollectionItemGetFullQuery(_collectionItemId), CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(CollectionItemFullDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_collectionItemEntity.Id);
        actual.Name.Should().Be(_collectionItemEntity.Name);
        actual.CollectionId.Should().Be(_collectionItemEntity.CollectionId);

        actual.Icon.Should().BeOfType(typeof(IconDto));
        actual.Icon.Should().NotBeNull();
        actual.Icon!.Id.Should().Be(_iconEntity.Id);
        actual.Icon.Title.Should().Be(_iconEntity.Title);
        actual.Icon.FileName.Should().Be(_iconEntity.FileName);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenCollectionItemDoesNotExists()
    {
        //arrange
        var request = new CollectionItemGetFullQuery(Guid.Empty);

        _uow.CollectionItemRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldReturnCollectionItemFullDtoWithEmptyIcon_WhenCollectionItemExists_ButIconDoesNotExists()
    {
        //arrange
        var request = new CollectionItemGetFullQuery(_collectionItemId);

        _uow.IconRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(CollectionItemFullDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_collectionItemEntity.Id);
        actual.Name.Should().Be(_collectionItemEntity.Name);
        actual.CollectionId.Should().Be(_collectionItemEntity.CollectionId);

        actual.Icon.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionItemGetFullQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}