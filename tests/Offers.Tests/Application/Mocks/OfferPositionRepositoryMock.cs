using NSubstitute;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;

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

    public static List<OfferPosition> InitialFakeDataSet => SeedData();

    public static List<OfferPosition> FakeDataSet { get; set; } = SeedData();

    public static IOfferPositionRepository GetRepository()
    {
        var repository = Substitute.For<IOfferPositionRepository>();
        
        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            FakeDataSet.FirstOrDefault(fake => fake.Id.Value == call.Arg<Guid>()));

        repository.GetByOffer(Arg.Any<Guid>(), CancellationToken.None).Returns(call =>
            FakeDataSet.Where(fake => fake.OfferId?.Value == call.Arg<Guid>()));

        repository.BulkInsert(Arg.Any<IEnumerable<OfferPosition>>(), CancellationToken.None).Returns(async call =>
        {
            var offerPositions = (call.Arg<IEnumerable<OfferPosition>>()).ToList();

            FakeDataSet.AddRange(offerPositions);

            return await repository.GetByOffer(offerPositions.First().OfferId?.Value ?? Guid.Empty, CancellationToken.None);
        });

        return repository;
    }
}