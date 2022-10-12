using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Domain;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.QueriesTests.CollectionItems;

public class CollectionItemGetByCollectionTests : CollectionItemTestsSetup
{
    private readonly CollectionItemGetByCollectionHandler _sut;
    private readonly CollectionItemGetAllHandler _getAll;

    private readonly Guid _collectionId;

    public CollectionItemGetByCollectionTests()
    {
        _sut = new CollectionItemGetByCollectionHandler(_uow, _mapper);
        _getAll = new CollectionItemGetAllHandler(_uow, _mapper);

        var all = _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult();
        _collectionId = all.First().CollectionId ?? Guid.Empty;
    }

    [Fact]
    public async Task ShouldReturnListOfCollectionItemDtoFromSet_WhenCollectionExists()
    {
        //arrange
        var request = new CollectionItemGetByCollectionQuery(_collectionId);

        //act
        var actual = (await _sut.Handle(request, CancellationToken.None)).ToList();

        var itemsByCollection =
            (await _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None)).Where(dto => dto.CollectionId == _collectionId);

        //assert
        actual.Should().AllBeOfType(typeof(CollectionItemDto));
        actual.Should().HaveCount(Constants.COLLECTION_ITEM_NUMBER);
        actual.Should().BeEquivalentTo(itemsByCollection);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenCollectionDoesNotExists()
    {
        //arrange
        var request = new CollectionItemGetByCollectionQuery(Guid.Empty);
        
        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionItemGetByCollectionQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}