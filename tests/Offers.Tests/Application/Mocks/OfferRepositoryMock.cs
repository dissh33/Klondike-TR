using NSubstitute;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;

namespace Offers.Tests.Application.Mocks;

public static class OfferRepositoryMock
{
    private static List<Offer> SeedData()
    {
        var data = new List<Offer>();

        for (int i = 0; i < 3; i++)
        {
            var offer = new Offer($"name{i}-{i}", $"msg{i}-{i}", null, OfferType.New, OfferStatus.Archived, Guid.NewGuid());
            data.Add(offer);
        }

        return data;
    }

    public static void ResetFakeDataSet()
    {
        FakeDataSet = InitialFakeDataSet;
    }

    public static List<Offer> InitialFakeDataSet => SeedData();

    public static List<Offer> FakeDataSet { get; set; } = SeedData();

    public static IOfferRepository GetRepository()
    {
        var repository = Substitute.For<IOfferRepository>();
        
        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            FakeDataSet.FirstOrDefault(fake => fake.Id.Value == call.Arg<Guid>()));

        repository.Insert(Arg.Any<Offer>(), CancellationToken.None).Returns(async call =>
        {
            var offer = call.Arg<Offer>();

            var newOffer = new Offer(
                offer.Title,
                offer.Message,
                offer.Expression,
                offer.Type, offer.Status,
                offer.Id.Value);

            FakeDataSet.Add(newOffer);

            return await repository.GetById(newOffer.Id.Value, CancellationToken.None);
        });

        return repository;
    }
}
