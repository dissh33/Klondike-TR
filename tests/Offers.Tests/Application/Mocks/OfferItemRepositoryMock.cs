using NSubstitute;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;

namespace Offers.Tests.Application.Mocks;

public static class OfferItemRepositoryMock
{
    public static IEnumerable<OfferItem> InitialFakeDataSet => SeedData();

    private static IEnumerable<OfferItem> SeedData()
    {
        var data = new List<OfferItem>();

        foreach (var offerPosition in OfferPositionRepositoryMock.InitialFakeDataSet)
        {
            for (int i = 0; i < 3; i++)
            {
                var offerItem = new OfferItem(offerPosition.Id, Guid.NewGuid(), 10, OfferItemType.Buy, Guid.NewGuid());
                data.Add(offerItem);
            }
        }

        return data;
    }

    public static IEnumerable<OfferItem> FakeDataSet { get; set; } = SeedData();

    public static IOfferItemRepository GetRepository()
    {
        var repository = Substitute.For<IOfferItemRepository>();

        //var fakeDataSet = SeedData().ToList();

        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            FakeDataSet.FirstOrDefault(fake => fake.Id.Value == call.Arg<Guid>()));

        return repository;
    }
}