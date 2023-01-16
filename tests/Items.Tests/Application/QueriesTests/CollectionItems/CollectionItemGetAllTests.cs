using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.QueriesTests.CollectionItems;

public class CollectionGetAllTests : CollectionItemTestsSetup
{
    private readonly CollectionItemGetAllHandler _sut;

    public CollectionGetAllTests()
    {
        _sut = new CollectionItemGetAllHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnListOfCollectionItemDtos_WhenAnyCollectionItemExists()
    {
        //act
        var actual = (await _sut.Handle(new CollectionItemGetAllQuery(), CancellationToken.None)).ToList();

        //assert
        actual.Should().HaveCount(CollectionItemRepositoryMock.InitialFakeDataSet.Count());
        actual.Should().AllBeOfType(typeof(CollectionItemDto));
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionItemGetAllQuery(), CancellationToken.None);
        
        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}