using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Domain;
using Items.Domain.Entities;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.QueriesTests.CollectionItems;

public class CollectionItemGetFullByCollectionTests : CollectionItemTestsSetup
{
    private readonly CollectionItemGetFullByCollectionHandler _sut;
    private readonly CollectionItemGetAllHandler _getAll;

    private readonly Guid _collectionId;
    private readonly IEnumerable<Icon> _icons;

    public CollectionItemGetFullByCollectionTests()
    {
        _sut = new CollectionItemGetFullByCollectionHandler(_uow, _mapper);
        _getAll = new CollectionItemGetAllHandler(_uow, _mapper);

        var all = _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().ToList();
        _collectionId = all.First().CollectionId ?? Guid.Empty;
        
        _icons = IconRepositoryMock.GetRepository().GetAll(CancellationToken.None).GetAwaiter().GetResult().Take(Constants.COLLECTION_ITEM_NUMBER);

        IconRepositoryMock.GetRepository().GetRange(Arg.Any<IEnumerable<Guid>>(), CancellationToken.None).Returns(_icons);
    }

    [Fact]
    public async Task ShouldReturnListOfCollectionItemFullWithFileDtoFromSet_WhenCollectionExists()
    {
        //arrange
        var request = new CollectionItemGetFullByCollectionQuery(_collectionId);
        
        //act
        var actual = (await _sut.Handle(request, CancellationToken.None)).ToList();

        var itemByCollection =
            (await _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None)).Where(x => x.CollectionId == _collectionId);

        //assert
        actual.Should().AllBeOfType(typeof(CollectionItemFullWithFileDto));
        actual.Should().HaveCount(Constants.COLLECTION_ITEM_NUMBER);

        actual.Select(fullDto => fullDto.Icon).Should().BeEquivalentTo(_icons, 
        options => options.Excluding(icon => icon.Id)
                                .Excluding(icon => icon.ExternalId));

        actual.Should().BeEquivalentTo(itemByCollection, options => options.Excluding(dto => dto.IconId));
    }

    [Fact]
    public async Task ShouldReturnNull_WhenCollectionDoesNotExists()
    {
        //arrange
        var request = new CollectionItemGetFullByCollectionQuery(Guid.Empty);
        
        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionItemGetFullByCollectionQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}