using NSubstitute;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using System.Linq;

namespace Offers.Tests.Application.Mocks;

public static class OfferPositionRepositoryMock
{
    private static List<OfferPosition> SeedData()
    {
        var data = new List<OfferPosition>();

        foreach (var offer in OfferRepositoryMock.InitialFakeDataSet)
        {
            for (int i = 0; i < 3; i++)
            {
                var offerPosition = new OfferPosition(offer.Id, $"rate{i}-to-{i + 1}", false, $"msg{i}-{i}", OfferPositionType.Buying, Guid.NewGuid());
                data.Add(offerPosition);
            }
        }

        return data;
    }

    public static void ResetFakeDataSet()
    {
        FakeDataSet = InitialFakeDataSet;
    }

    public static List<OfferPosition> InitialFakeDataSet => SeedData();

    public static List<OfferPosition> FakeDataSet { get; set; } = SeedData();

    public static IOfferPositionRepository GetRepository()
    {
        var repository = Substitute.For<IOfferPositionRepository>();
        
        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            FakeDataSet.FirstOrDefault(fake => fake.Id.Value == call.Arg<Guid>()));

        repository.GetByOffer(Arg.Any<Guid>(), CancellationToken.None).Returns(call =>
            FakeDataSet.Where(fake => fake.OfferId?.Value == call.Arg<Guid>()));

        repository.GetByOffers(Arg.Any<IEnumerable<Guid>>(), CancellationToken.None).Returns(call =>
            FakeDataSet.Where(fake => call.Arg<IEnumerable<Guid>>().ToList().Contains(fake.OfferId?.Value ?? Guid.NewGuid())));

        repository.BulkInsert(Arg.Any<IEnumerable<OfferPosition>>(), CancellationToken.None).Returns(async call =>
        {
            var offerPositions = call.Arg<IEnumerable<OfferPosition>>().ToList();

            var newPositions = offerPositions.Select(position => 
                new OfferPosition(
                    position.OfferId, 
                    position.PriceRate, 
                    position.WithTrader, 
                    position.Message, 
                    position.Type, 
                    position.Id.Value))
                .ToList();

            FakeDataSet.AddRange(newPositions);

            return await repository.GetByOffer(newPositions.FirstOrDefault()?.OfferId?.Value ?? Guid.Empty, CancellationToken.None);
        });

        return repository;
    }
}