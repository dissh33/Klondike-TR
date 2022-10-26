using NSubstitute;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;

namespace Offers.Tests.Application.Mocks;

public static class OfferPositionRepositoryMock
{
    public static IEnumerable<OfferPosition> InitialFakeDataSet => SeedData();

    private static IEnumerable<OfferPosition> SeedData()
    {
        var data = new List<OfferPosition>();

        foreach (var offer in OfferRepositoryMock.InitialFakeDataSet)
        {
            for (int i = 0; i < 3; i++)
            {
                var offerPosition = new OfferPosition(offer.Id, $"rate{i}-to-{i+1}", false, $"msg{i}-{i}", OfferPositionType.Buying, Guid.NewGuid());
                data.Add(offerPosition);
            } 
        }

        return data;
    }

    public static IOfferPositionRepository GetRepository()
    {
        var repository = Substitute.For<IOfferPositionRepository>();

        var fakeDataSet = SeedData().ToList();

        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            fakeDataSet.FirstOrDefault(fake => fake.Id.Value == call.Arg<Guid>()));

        return repository;
    }
}