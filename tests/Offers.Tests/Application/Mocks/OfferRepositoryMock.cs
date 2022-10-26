using NSubstitute;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;

namespace Offers.Tests.Application.Mocks;

public static class OfferRepositoryMock
{
    public static IEnumerable<Offer> InitialFakeDataSet => SeedData();

    private static IEnumerable<Offer> SeedData()
    {
        var data = new List<Offer>();

        for (int i = 0; i < 3; i++)
        {
            var offer = new Offer($"name{i}-{i}", $"msg{i}-{i}", null, OfferType.New, OfferStatus.Archived, Guid.NewGuid());
            data.Add(offer);
        }

        return data;
    }

    public static IEnumerable<Offer> FakeDataSet { get; set; } = SeedData();

    public static IOfferRepository GetRepository()
    {
        var repository = Substitute.For<IOfferRepository>();

        //var fakeDataSet = SeedData().ToList();
        
        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            FakeDataSet.FirstOrDefault(fake => fake.Id.Value == call.Arg<Guid>()));

        return repository;
    }
}
