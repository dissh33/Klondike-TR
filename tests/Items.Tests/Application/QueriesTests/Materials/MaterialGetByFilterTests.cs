using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Materials;

public class CollectionGetByFilterTests : MaterialTestsSetup
{
    private readonly MaterialGetByFilterHandler _sut;

    public CollectionGetByFilterTests()
    {
        _sut = new MaterialGetByFilterHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnListOfMaterialDtos_WhenMaterialsWithParametersFromFilterExists()
    {
        //assert
        var query = new MaterialGetByFilterQuery
        {
            Name = "name1",
            Type = 0,
            Status = 1,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(MaterialDto));
        actual.Should().HaveCount(1);
    }

    [Fact]
    public async Task ShouldReturnEmpty_WhenMaterialsWithParametersFromFilterDoesNotExists()
    {
        //assert
        var query = new MaterialGetByFilterQuery
        {
            Name = "name",
            Type = 0,
            Status = 0,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(MaterialDto));
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnListOfMaterialDtos_WhenMaterialsWithNameFromFilterExists()    
    {
        //assert
        var query = new MaterialGetByFilterQuery
        {
            Name = "NaMe",
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(MaterialDto));
        actual.Should().HaveCount(3);
    }

    [Fact]
    public async Task ShouldReturnEmptyList_WhenMaterialsWithNameFromFilterDoesNotExists()
    {
        //assert
        var query = new MaterialGetByFilterQuery
        {
            Name = "not-exists",
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(MaterialDto));
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnListOfMaterialDtos_WhenMaterialsWithTypeFromFilterExists()
    {
        //assert
        var query = new MaterialGetByFilterQuery
        {
            Type = 0,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(MaterialDto));
        actual.Should().HaveCount(3);
    }

    [Fact]
    public async Task ShouldReturnEmptyList_WhenMaterialsWithTypeFromFilterDoesNotExists()
    {
        //assert
        var query = new MaterialGetByFilterQuery
        {
            Type = 1,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(MaterialDto));
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnListOfMaterialDtos_WhenMaterialsWithStatusFromFilterExists()
    {
        //assert
        var query = new MaterialGetByFilterQuery
        {
            Status = 1,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(MaterialDto));
        actual.Should().HaveCount(3);
    }

    [Fact]
    public async Task ShouldReturnEmptyList_WhenMaterialsWithStatusFromFilterDoesNotExists()
    {
        //assert
        var query = new MaterialGetByFilterQuery
        {
            Status = 2,
        };

        //act
        var actual = (await _sut.Handle(query, CancellationToken.None)).ToList();

        //assert
        actual.Should().AllBeOfType(typeof(MaterialDto));
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new MaterialGetByFilterQuery(), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}