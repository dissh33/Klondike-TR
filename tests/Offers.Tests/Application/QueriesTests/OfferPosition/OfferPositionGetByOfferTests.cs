using FluentAssertions;
using NSubstitute;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferPosition;
using Offers.Application.QueryHandlers.OfferPosition;
using Offers.Tests.Application.Mocks;
using Offers.Tests.Application.Setup;
using Xunit;

namespace Offers.Tests.Application.QueriesTests.OfferPosition;

public class OfferPositionGetByOfferTests : OfferTestsSetup
{
    private readonly OfferPositionGetByOfferHandler _sut;

    public OfferPositionGetByOfferTests()
    {
        _sut = new OfferPositionGetByOfferHandler(_uow, _mapper);
    }

    [Fact]
    internal async Task ShouldReturnListOfOfferPositionDtoFromSet_WhenOfferExists()
    {
        //arrange
        var offerId = OfferPositionRepositoryMock.FakeDataSet.First().OfferId;

        //act
        var actual = (await _sut.Handle(new OfferPositionGetByOfferQuery(offerId!.Value), CancellationToken.None)).ToList();

        var itemsByPosition = OfferPositionRepositoryMock.FakeDataSet.Where(x => x.OfferId == offerId).ToList();

        //assert
        actual.Should().HaveCount(3);
        actual.Should().AllBeOfType(typeof(OfferPositionDto));

        actual.Select(dto => dto.OfferId).Should().AllBeEquivalentTo(offerId.Value);
        actual.Select(dto => dto.Id).Should().BeEquivalentTo(itemsByPosition.Select(position => position.Id.Value));
        actual.Select(dto => dto.Type).Should().BeEquivalentTo(itemsByPosition.Select(position => (int)position.Type));

        actual.Should().BeEquivalentTo(itemsByPosition, options => options
            .Excluding(position => position.OfferId)
            .Excluding(position => position.Id)
            .Excluding(position => position.Type)
            .Excluding(position => position.CreateDate)
            .Excluding(position => position.DomainEvents));
    }

    [Fact]
    internal async Task ShouldReturnEmpty_WhenOfferDoesNotExists()
    {
        //arrange
        var request = new OfferPositionGetByOfferQuery(Guid.Empty);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeEmpty();
    }

    [Fact]
    internal async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new OfferPositionGetByOfferQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}
