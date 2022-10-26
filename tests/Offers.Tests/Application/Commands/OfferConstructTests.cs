using AutoMapper;
using NSubstitute;
using Offers.Api.Commands;
using Offers.Api.Dtos;
using Offers.Application.CommandHandlers;
using Offers.Application.Contracts;
using Offers.Application.Mapping;
using Offers.Domain.Enums;
using Offers.Tests.Application.Mocks;
using Xunit;

namespace Offers.Tests.Application.Commands;

public class OfferConstructTests
{
    private readonly IUnitOfWork _uow;

    private readonly OfferConstructHandler _sut;

    private readonly OfferConstructCommand _command;

    public OfferConstructTests()
    {
        var mapperConfig = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile<OfferProfile>();
            configuration.AddProfile<OfferPositionProfile>();
            configuration.AddProfile<OfferItemProfile>();
        });

        var mapper = mapperConfig.CreateMapper();
        _uow = UnitOfWorkMock.GetUnitOfWork();

        _sut = new OfferConstructHandler(_uow, mapper);

        _command = new OfferConstructCommand
        {
            Title = "title-new",
            Message = "msg-new",
            Type = (int) OfferType.New,
            Status = (int) OfferStatus.Disable,
            Positions = new List<OfferPositionDto>
            {
                new()
                {
                    Message = "msg-pos-new",
                    PriceRate = "1to10-new",
                    WithTrader = false,
                    Type = (int) OfferPositionType.Buying,
                    OfferItems = new List<OfferItemDto>
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
                    PriceRate = "101to3-new-2",
                    WithTrader = false,
                    Type = (int) OfferPositionType.Buying,
                    OfferItems = new List<OfferItemDto>
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

    [Fact]
    public async Task ShouldAddNewEntitiesToTheirSets()
    {
    }

    [Fact]
    public async Task ShouldReturnAddedOfferDto()
    {
    }

    [Fact]
    public async Task ShouldRollbackAndReturnNull_WhenCantAddOfferEntity()
    {
    }

    [Fact]
    public async Task ShouldReturnOfferDto_WithValuesFromCommand()
    {
    }

    [Fact]
    public async Task ShouldReturnOfferDto_WithEmptyPositions_WhenPositionsNotSpecified()
    {
    }

    [Fact]
    public async Task ShouldReturnOfferDto_WithEmptyOfferItems_WhenItemsNotSpecified()
    {
    }


    [Fact]
    public async Task ShouldCommitOnce()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);

        //assert
        _uow.Received(1).Commit();
    }
}