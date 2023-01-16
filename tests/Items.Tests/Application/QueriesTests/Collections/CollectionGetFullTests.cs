using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Collection;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.Collection;
using Items.Application.QueryHandlers.CollectionHandlers;
using Items.Domain;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Domain.Exceptions;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setup;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Collections;

public class CollectionGetFullTests : CollectionTestsSetup
{
    private readonly CollectionGetFullHandler _sut;

    private readonly Guid _collectionId;
    private readonly Collection _collectionEntity;
    private readonly Icon _collectionIcon;
    private readonly List<CollectionItem> _collectionItemsEntities;

    public CollectionGetFullTests()
    {
        _sut = new CollectionGetFullHandler(_uow, _mapper);
        
        _collectionId = Guid.NewGuid();

        _collectionItemsEntities = new List<CollectionItem>();
        for (int i = 0; i < Constants.COLLECTION_ITEM_NUMBER; i++)
        {
            var item = new CollectionItem($"full-name-{i}", _collectionId);
            _collectionItemsEntities.Add(item);
        }

        _collectionEntity = new Collection("n1", status: ItemStatus.Disabled, id: _collectionId);
        _collectionIcon = new Icon("collection-icon", Array.Empty<byte>(), "fn-collection");
        
        var iconEntities = IconRepositoryMock.InitialFakeDataSet.Take(Constants.COLLECTION_ITEM_NUMBER);
        _uow.IconRepository.GetRange(Arg.Any<IEnumerable<Guid>>(), CancellationToken.None).Returns(iconEntities);
        _uow.IconRepository.GetById(_collectionIcon.Id, CancellationToken.None).Returns(_collectionIcon);

        _uow.CollectionRepository.GetById(_collectionId, CancellationToken.None).Returns(_collectionEntity);
        _uow.CollectionItemRepository.GetByCollection(Arg.Any<Guid>(), CancellationToken.None).Returns(_collectionItemsEntities);
    }

    [Fact]
    public async Task ShouldReturnCorrectCollectionFullDto_WhenCollectionAndCollectionItemsAndIconExist()
    {
        //arrange
        _collectionEntity.AddIcon(_collectionIcon.Id);

        //act
        var actual = await _sut.Handle(new CollectionGetFullQuery(_collectionId), CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(CollectionFullDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_collectionEntity.Id);
        actual.Name.Should().Be(_collectionEntity.Name);
        actual.Status.Should().Be((int) _collectionEntity.Status!);

        actual.Icon.Should().BeOfType(typeof(IconDto));
        actual.Icon.Should().BeEquivalentTo(_collectionIcon,
            options => options.Excluding(icon => icon.ExternalId).Excluding(icon => icon.FileBinary));

        actual.Items.Should().AllBeOfType(typeof(CollectionItemFullDto));
        actual.Items.Should().HaveCount(Constants.COLLECTION_ITEM_NUMBER);
        actual.Items!.Select(item => item.Icon).Should().NotBeNull();
        actual.Items.Should().BeEquivalentTo(_collectionItemsEntities, 
        options => options
                .Excluding(item => item.IconId)
                .Excluding(item => item.ExternalId)
                .Excluding(item => item.Icon!.ExternalId)
                .Excluding(item => item.Icon!.FileBinary));
    }

    [Fact]
    public async Task ShouldReturnNull_WhenCollectionDoesNotExists()
    {
        //arrange
        var request = new CollectionGetFullQuery(Guid.Empty);

        _uow.CollectionRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldThrowException_WhenCollectionItemsCountIsNotFive()
    {
        //arrange
        var request = new CollectionGetFullQuery(_collectionId);

        _uow.CollectionItemRepository.GetByCollection(_collectionId, CancellationToken.None).Returns(Enumerable.Empty<CollectionItem>());

        //act
        var act = () => _sut.Handle(request, CancellationToken.None);

        //assert
        await act.Should().ThrowAsync<WrongCollectionItemsNumberException>();
    }

    [Fact]
    public async Task ShouldReturnCollectionFullDtoWithEmptyIcon_WhenCollectionExists_ButIconDoesNotExists()
    {
        //arrange
        var request = new CollectionGetFullQuery(_collectionId);

        _uow.IconRepository.GetById(_collectionIcon.Id, CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(CollectionFullDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_collectionEntity.Id);
        actual.Name.Should().Be(_collectionEntity.Name);
        actual.Status.Should().Be((int)_collectionEntity.Status!);

        actual.Items.Should().AllBeOfType(typeof(CollectionItemFullDto));
        actual.Items.Should().HaveCount(Constants.COLLECTION_ITEM_NUMBER);

        actual.Icon.Should().BeNull();
    }

    [Fact]
    public async Task ShouldReturnCollectionFullDtoWithEmptyIconsInCollectionItems_WhenCollectionExists_ButIconsInCollectionItemsDoesNotExists()
    {
        //arrange
        var request = new CollectionGetFullQuery(_collectionId);

        _collectionEntity.AddIcon(_collectionIcon.Id);
        _uow.IconRepository.GetRange(Arg.Any<IEnumerable<Guid>>(), CancellationToken.None).Returns(new List<Icon>());

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(CollectionFullDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(_collectionEntity.Id);
        actual.Name.Should().Be(_collectionEntity.Name);
        actual.Status.Should().Be((int)_collectionEntity.Status!);

        actual.Icon.Should().BeOfType(typeof(IconDto));
        actual.Icon.Should().BeEquivalentTo(_collectionIcon,
            options => options.Excluding(icon => icon.Id).Excluding(icon => icon.ExternalId).Excluding(icon => icon.FileBinary));

        actual.Items.Should().AllBeOfType(typeof(CollectionItemFullDto));
        actual.Items.Should().HaveCount(Constants.COLLECTION_ITEM_NUMBER);
        actual.Items.Should().BeEquivalentTo(_collectionItemsEntities,
            options => options
                .Excluding(item => item.Id)
                .Excluding(item => item.CollectionId)
                .Excluding(item => item.IconId)
                .Excluding(item => item.ExternalId)
                .Excluding(item => item.Icon));

        actual.Items!.Select(item => item.Icon).Should().BeEquivalentTo(new List<Icon?> {null, null, null, null, null} );
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionGetFullQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}