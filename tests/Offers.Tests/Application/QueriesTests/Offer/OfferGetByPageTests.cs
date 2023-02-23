using FluentAssertions;
using NSubstitute;
using Offers.Api.Dtos;
using Offers.Api.Helpers;
using Offers.Api.Queries.Offer;
using Offers.Application.QueryHandlers.OfferHandlers;
using Offers.Tests.Application.Mocks;
using Offers.Tests.Application.Setup;
using Xunit;

namespace Offers.Tests.Application.QueriesTests.Offer;

[Collection("OfferSequentially")]
public class OfferGetByPageTests : OfferTestsSetup
{
    private readonly OfferGetByPageHandler _sut;

    public OfferGetByPageTests()
    {
        _sut = new OfferGetByPageHandler(_uow, _mapper);
    }

    [Fact]
    internal async Task ShouldReturnCorrectPaginationWrapper_WhenPassedCorrectParameters()
    {
        //arrange
        ResetData();
        var request = new OfferGetByPageQuery()
        {
            Page = 1,
            PageSize = 2,
        };

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(PaginationWrapper<OfferDto>));
        actual.Should().NotBeNull();

        actual!.Data.Should().HaveCount(request.PageSize);
        actual.Page.Should().Be(request.Page);
        actual.PageSize.Should().Be(request.PageSize);
        actual.TotalItems.Should().Be(OfferRepositoryMock.InitialFakeDataSet.Count);
        actual.TotalPages.Should().Be(2);
        actual.HasPrevious.Should().BeFalse();
        actual.HasNext.Should().BeTrue();
    }

    [Fact]
    internal async Task ShouldReturnPaginationWrapperWithCorrectDataInside_WhenOffersExistsAndPassedCorrectParameters()
    {
        //arrange
        ResetData();
        var request = new OfferGetByPageQuery()
        {
            Page = 2,
            PageSize = 1,
        };

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(PaginationWrapper<OfferDto>));
        actual.Should().NotBeNull();

        actual!.Data.Should().HaveCount(request.PageSize);
        actual.Data.First().Id.Should().Be(OfferRepositoryMock.FakeDataSet[1].Id.Value);
        actual.Data.First().Should().BeEquivalentTo(OfferRepositoryMock.FakeDataSet[1], options => options
            .Excluding(offer => offer.Id)
            .Excluding(offer => offer.CreateDate)
            .Excluding(offer => offer.DomainEvents)
            .Excluding(offer => offer.Status)
            .Excluding(offer => offer.Type));
    }

    [Fact]
    internal async Task ShouldReturnPaginationWrapperWithOrderedDataInside_WhenPassedCorrectOrderByParameter()
    {
        //arrange
        ResetData();
        var request = new OfferGetByPageQuery()
        {
            Page = 1,
            PageSize = 2,
            OrderBy = new Dictionary<string, string?>
            {
                { "title", "" },
            },
        };

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(PaginationWrapper<OfferDto>));
        actual.Should().NotBeNull();

        actual!.Data.Should().HaveCount(request.PageSize);
        actual.Page.Should().Be(request.Page);
        actual.PageSize.Should().Be(request.PageSize);
        actual.TotalItems.Should().Be(OfferRepositoryMock.InitialFakeDataSet.Count);
        actual.TotalPages.Should().Be(2);
        actual.HasPrevious.Should().BeFalse();
        actual.HasNext.Should().BeTrue();

        actual.Data.First().Id.Should().Be(OfferRepositoryMock.FakeDataSet[0].Id.Value);
        actual.Data.First().Should().BeEquivalentTo(OfferRepositoryMock.FakeDataSet[0], options => options
            .Excluding(offer => offer.Id)
            .Excluding(offer => offer.CreateDate)
            .Excluding(offer => offer.DomainEvents)
            .Excluding(offer => offer.Status)
            .Excluding(offer => offer.Type));
    }

    [Fact]
    internal async Task ShouldReturnNull_WhenPassedIncorrectParameters()
    {
        //arrange
        var request = new OfferGetByPageQuery
        {
            Page = 10,
            PageSize = 10,
        };

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    internal async Task ShouldNotCommit()
    {
        //arrange
        var request = new OfferGetByPageQuery
        {
            Page = 1,
            PageSize = 3,
        };

        //act
        await _sut.Handle(request, CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}
