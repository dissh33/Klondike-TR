using FluentAssertions;
using NSubstitute;
using Offers.Api.Dtos;
using Offers.Api.Queries.OfferPosition;
using Offers.Application.QueryHandlers.OfferPosition;
using Offers.Domain.Enums;
using Offers.Domain.TypedIds;
using Offers.Tests.Application.Setup;
using Xunit;

namespace Offers.Tests.Application.QueriesTests.OfferPosition;

public class OfferPositionGetByIdTests : OfferTestsSetup
{
    private readonly OfferPositionGetByIdHandler _sut;

    public OfferPositionGetByIdTests()
    {
        _sut = new OfferPositionGetByIdHandler(_uow, _mapper);
    }

    [Fact]
    internal async Task ShouldReturnCorrectOfferPositionDto_WhenOfferPositionExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new OfferPositionGetByIdQuery(id);
        var offerPositionEntity = new Offers.Domain.Entities.OfferPosition(new OfferId(Guid.NewGuid()), "price-1", false, "m-1", OfferPositionType.ExactOffer, id);

        _uow.OfferPositionRepository.GetById(id, CancellationToken.None).Returns(offerPositionEntity);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(OfferPositionDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(offerPositionEntity.Id.Value);
        actual.OfferId.Should().Be(offerPositionEntity.OfferId!.Value);
        actual.PriceRate.Should().Be(offerPositionEntity.PriceRate);
        actual.WithTrader.Should().Be(offerPositionEntity.WithTrader);
        actual.Message.Should().Be(offerPositionEntity.Message);
        actual.Type.Should().Be((int)offerPositionEntity.Type);
    }

    [Fact]
    internal async Task ShouldReturnNull_WhenOfferPositionDoesNotExists()
    {
        //arrange
        var request = new OfferPositionGetByIdQuery(Guid.Empty);
        
        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    internal async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new OfferPositionGetByIdQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}
