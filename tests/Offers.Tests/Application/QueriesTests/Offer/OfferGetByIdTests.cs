using FluentAssertions;
using NSubstitute;
using Offers.Api.Dtos;
using Offers.Api.Queries.Offer;
using Offers.Application.QueryHandlers.OfferHandlers;
using Offers.Domain.Enums;
using Xunit;
using Offers.Tests.Application.Setup;

namespace Offers.Tests.Application.QueriesTests.Offer;

public class OfferGetByIdTests : OfferTestsSetup
{
    private readonly OfferGetByIdHandler _sut;

    public OfferGetByIdTests()
    {
        _sut = new OfferGetByIdHandler(_uow, _mapper);
    }

    [Fact]
    internal async Task ShouldReturnCorrectOfferDto_WhenOfferExists()
    {
        //arrange
        var id = Guid.NewGuid();
        var request = new OfferGetByIdQuery(id);
        var offerEntity = new Offers.Domain.Entities.Offer("t-1", "m-1", "e-1", OfferType.New, OfferStatus.Archived, id);

        _uow.OfferRepository.GetById(id, CancellationToken.None).Returns(offerEntity);

        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeOfType(typeof(OfferDto));
        actual.Should().NotBeNull();

        actual!.Id.Should().Be(offerEntity.Id.Value);
        actual.Title.Should().Be(offerEntity.Title);
        actual.Message.Should().Be(offerEntity.Message);
        actual.Expression.Should().Be(offerEntity.Expression);
        actual.Type.Should().Be((int)offerEntity.Type);
        actual.Status.Should().Be((int)offerEntity.Status);
    }

    [Fact]
    internal async Task ShouldReturnNull_WhenOfferDoesNotExists()
    {
        //arrange
        var request = new OfferGetByIdQuery(Guid.Empty);
        
        //act
        var actual = await _sut.Handle(request, CancellationToken.None);

        //assert
        actual.Should().BeNull();
    }

    [Fact]
    internal async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new OfferGetByIdQuery(Guid.Empty), CancellationToken.None);

        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}
