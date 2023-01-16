using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Collection;
using Items.Api.Queries.Collection;
using Items.Application.QueryHandlers.CollectionHandlers;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Collections;

public class CollectionGetByFilterTests : CollectionTestsSetup
{
    private readonly CollectionGetByFilterHandler _sut;

    public CollectionGetByFilterTests()
    {
        _sut = new CollectionGetByFilterHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnListOfCollectionDtos_WhenCollectionsWithParametersFromFilterExists()
    {
        //assert
        var query = new CollectionGetByFilterQuery
        {
            Name = "name1",
            Status = 1,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(CollectionDto));
        actual.Should().HaveCount(1);
    }

    [Fact]
    public async Task ShouldReturnEmpty_WhenCollectionsWithParametersFromFilterDoesNotExists()
    {
        //assert
        var query = new CollectionGetByFilterQuery
        {
            Name = "name",
            Status = 0,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(CollectionDto));
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnListOfCollectionDtos_WhenCollectionsWithNameFromFilterExists()    
    {
        //assert
        var query = new CollectionGetByFilterQuery
        {
            Name = "NaMe",
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(CollectionDto));
        actual.Should().HaveCount(3);
    }

    [Fact]
    public async Task ShouldReturnEmptyList_WhenCollectionsWithNameFromFilterDoesNotExists()
    {
        //assert
        var query = new CollectionGetByFilterQuery
        {
            Name = "not-exists",
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(CollectionDto));
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnListOfCollectionDtos_WhenCollectionsWithStatusFromFilterExists()
    {
        //assert
        var query = new CollectionGetByFilterQuery
        {
            Status = 1,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(CollectionDto));
        actual.Should().HaveCount(3);
    }

    [Fact]
    public async Task ShouldReturnEmptyList_WhenCollectionsWithStatusFromFilterDoesNotExists()
    {
        //assert
        var query = new CollectionGetByFilterQuery
        {
            Status = 2,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(CollectionDto));
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new CollectionGetByFilterQuery(), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}