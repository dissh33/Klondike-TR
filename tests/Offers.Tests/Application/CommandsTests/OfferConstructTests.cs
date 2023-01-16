using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Offers.Api.Commands;
using Offers.Api.Dtos;
using Offers.Application.CommandHandlers;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.TypedIds;
using Offers.Tests.Application.Mocks;
using Offers.Tests.Application.Setup;
using Xunit;

namespace Offers.Tests.Application.CommandsTests;

public class OfferConstructTests : OfferTestsSetup
{
    private readonly OfferConstructHandler _sut;
    
    private readonly OfferConstructCommand _command;

    public OfferConstructTests()
    {
        _sut = new OfferConstructHandler(_uow, _mapper);

        _command = new OfferConstructCommand
        {
            Title = "title-new",
            Message = "msg-new",
            Type = (int) OfferType.New,
            Status = (int) OfferStatus.Disable,
            Positions = new List<OfferPositionAddDto>
            {
                new()
                {
                    Message = "msg-pos-new",
                    PriceRate = "1to10-new",
                    WithTrader = false,
                    Type = (int) OfferPositionType.Buying,
                    OfferItems = new List<OfferItemAddDto>
                    {
                        new()
                        {
                            TradableItemId = Guid.NewGuid(),
                            Amount = 1,
                            Type = (int) OfferItemType.Sell,
                        },
                        new()
                        {
                            TradableItemId = Guid.NewGuid(),
                            Amount = 10,
                            Type = (int) OfferItemType.Buy,
                        },
                    },
                },
                new()
                {
                    Message = "msg-pos-new-2",
                    PriceRate = "10to3-new-2",
                    WithTrader = false,
                    Type = (int) OfferPositionType.Buying,
                    OfferItems = new List<OfferItemAddDto>
                    {
                        new()
                        {
                            TradableItemId = Guid.NewGuid(),
                            Amount = 10,
                            Type = (int) OfferItemType.Sell,
                        },
                        new()
                        {
                            TradableItemId = Guid.NewGuid(),
                            Amount = 3,
                            Type = (int) OfferItemType.Buy,
                        },
                    },
                },
            },
        };
    }

    private static void ResetData()
    {
        OfferRepositoryMock.ResetFakeDataSet();
        OfferPositionRepositoryMock.ResetFakeDataSet();
        OfferItemRepositoryMock.ResetFakeDataSet();
    }

    [Fact]
    internal async Task ShouldAddNewEntitiesToTheirSets()
    {
        //arrange
        ResetData();

        //act
        await _sut.Handle(_command, CancellationToken.None);
        
        //assert
        OfferRepositoryMock.FakeDataSet.Should().HaveCount(OfferRepositoryMock.InitialFakeDataSet.Count + 1);
        OfferPositionRepositoryMock.FakeDataSet.Should().HaveCount(OfferPositionRepositoryMock.InitialFakeDataSet.Count + 2);
        OfferItemRepositoryMock.FakeDataSet.Should().HaveCount(OfferItemRepositoryMock.InitialFakeDataSet.Count + 4);
    }

    [Fact]
    internal async Task ShouldReturnAddedOfferDto() //TODO: DOES NOT WORK YET
    {
        //arrange
        ResetData();

        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //TODO: with OfferGetByIdHandler
        var added = OfferRepositoryMock.FakeDataSet.FirstOrDefault(offer => offer.Id == new OfferId(actual?.Id ?? Guid.Empty));

        //assert
        actual.Should().BeOfType<OfferDto>();
        added.Should().NotBeNull();

        added.Should().BeEquivalentTo(actual, options => options.Excluding(dto => dto!.Positions));
    }

    [Fact]
    internal async Task ShouldRollbackAndReturnNull_WhenCantAddOfferEntity()
    {
        //arrange
        ResetData();

        _uow.OfferRepository.Insert(Arg.Any<Offer>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        _uow.Received(1).Rollback();
        actual.Should().BeNull();
    }

    [Fact]
    internal async Task ShouldReturnOfferDto_WithValuesFromCommand()
    {
    }

    [Fact]
    internal async Task ShouldReturnOfferDto_WithEmptyPositions_WhenPositionsNotSpecified()
    {
        //arrange
        ResetData();

        var specificCommand = _command;
        specificCommand.Positions = new List<OfferPositionAddDto>();

        //act
        var actual = await _sut.Handle(specificCommand, CancellationToken.None);
    }

    [Fact]
    internal async Task ShouldThrow_MissingOfferItemsException_WhenItemsNotSpecified()
    {
        //Need this?
    }


    [Fact]
    internal async Task ShouldCommitOnce()
    {
        //arrange
        ResetData();

        //act
        await _sut.Handle(_command, CancellationToken.None);

        //assert
        _uow.Received(1).Commit();
    }
}