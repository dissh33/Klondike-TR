using FluentAssertions;
using NSubstitute;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferItem;
using Offers.Application.QueryHandlers.OfferItem;
using Offers.Domain.Enums;
using Offers.Domain.TypedIds;
using Offers.Tests.Application.Setup;
using Xunit;

namespace Offers.Tests.Application.QueriesTests.OfferItem;

public class OfferItemGetByIdTests : OfferTestsSetup
{
    private readonly OfferItemGetByIdHandler _sut;

    public OfferItemGetByIdTests()
    {
        _sut = new OfferItemGetByIdHandler(_uow, _mapper);
    }

    [Fact]
    internal async Task ShouldReturnCorrectOfferItemDto_WhenOfferItemExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new OfferItemGetByIdQuery(id);
        var offerItemEntity = new Offers.Domain.Entities.OfferItem(new OfferPositionId(Guid.NewGuid()), Guid.NewGuid(), 101, OfferItemType.Sell, id);

        _uow.OfferItemRepository.GetById(id, CancellationToken.None).Returns(offerItemEntity);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(OfferItemDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(offerItemEntity.Id.Value);
        actual.OfferPositionId.Should().Be(offerItemEntity.OfferPositionId!.Value);
        actual.TradableItemId.Should().Be(offerItemEntity.TradableItemId);
        actual.Amount.Should().Be(offerItemEntity.Amount);
        actual.Type.Should().Be((int)offerItemEntity.Type);
    }

    [Fact]
    internal async Task ShouldReturnNull_WhenOfferItemDoesNotExists()
    {
        //arrange
        var request = new OfferItemGetByIdQuery(Guid.Empty);
        
        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    internal async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new OfferItemGetByIdQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}
