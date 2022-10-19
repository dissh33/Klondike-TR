using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Collection;
using Items.Api.Queries.Collection;
using Items.Application.QueryHandlers.CollectionHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Collectionы;

public class CollectionGetAllTests : CollectionTestsSetup
{
    private readonly CollectionGetAllHandler _sut;

    public CollectionGetAllTests()
    {
        _sut = new CollectionGetAllHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnListOfCollectionDtos_WhenAnyCollectionExists()
    {
        //act
        var actual = (await _sut.Handle(new CollectionGetAllQuery(), CancellationToken.None)).ToList();

        //assert
        actual.Should().HaveCount(CollectionRepositoryMock.InitialFakeDataSet.Count());
        actual.Should().AllBeOfType(typeof(CollectionDto));
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionGetAllQuery(), CancellationToken.None);
        
        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}