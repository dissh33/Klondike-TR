using FluentAssertions;
using NSubstitute;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferItem;
using Offers.Application.QueryHandlers.OfferItem;
using Offers.Tests.Application.Mocks;
using Offers.Tests.Application.Setup;
using Xunit;

namespace Offers.Tests.Application.QueriesTests.OfferItem;

[Collection("Offer-Sequentially")]
public class OfferItemGetByPositionTests : OfferTestsSetup
{
    private readonly OfferItemGetByPositionHandler _sut;

    public OfferItemGetByPositionTests()
    {
        _sut = new OfferItemGetByPositionHandler(_uow, _mapper);
    }

    [Fact]
    internal async Task ShouldReturnListOfOfferItemDtoFromSet_WhenOfferPositionExists()
    {
        //arrange
        OfferItemRepositoryMock.ResetFakeDataSet();
        var offerPositionId = OfferItemRepositoryMock.FakeDataSet.First().OfferPositionId;

        //act
        var actual = (await _sut.Handle(new OfferItemGetByPositionQuery(offerPositionId!.Value), CancellationToken.None)).ToList();

        var itemsByPosition = OfferItemRepositoryMock.FakeDataSet.Where(x => x.OfferPositionId == offerPositionId).ToList();

        //assert
        actual.Should().HaveCount(3);
        actual.Should().AllBeOfType(typeof(OfferItemDto));

        actual.Select(dto => dto.OfferPositionId).Should().AllBeEquivalentTo(offerPositionId.Value);
        actual.Select(dto => dto.Id).Should().BeEquivalentTo(itemsByPosition.Select(offerItem => offerItem.Id.Value));
        actual.Select(dto => dto.Type).Should().BeEquivalentTo(itemsByPosition.Select(offerItem => (int)offerItem.Type));

        actual.Should().BeEquivalentTo(itemsByPosition, options => options
            .Excluding(offerItem => offerItem.OfferPositionId)
            .Excluding(offerItem => offerItem.Id)
            .Excluding(offerItem => offerItem.Type)
            .Excluding(offerItem => offerItem.CreateDate)
            .Excluding(offerItem => offerItem.DomainEvents));
    }

    [Fact]
    internal async Task ShouldReturnEmpty_WhenOfferPositionDoesNotExists()
    {
        //arrange
        var request = new OfferItemGetByPositionQuery(Guid.Empty);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeEmpty();
    }

    [Fact]
    internal async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new OfferItemGetByPositionQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}
