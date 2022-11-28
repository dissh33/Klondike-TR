using NSubstitute;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;

namespace Offers.Tests.Application.Mocks;

public static class OfferItemRepositoryMock
{
    private static List<OfferItem> SeedData()
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

    public static void ResetFakeDataSet()
    {
        FakeDataSet = InitialFakeDataSet;
    }

    public static List<OfferItem> InitialFakeDataSet => SeedData();
    
    public static List<OfferItem> FakeDataSet { get; set; } = SeedData();

    public static IOfferItemRepository GetRepository()
    {
        var repository = Substitute.For<IOfferItemRepository>();
        
        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            FakeDataSet.FirstOrDefault(fake => fake.Id.Value == call.Arg<Guid>()));

        repository.GetByPosition(Arg.Any<Guid>(), CancellationToken.None).Returns(call =>
            FakeDataSet.Where(fake => fake.OfferPositionId?.Value == call.Arg<Guid>()));

        repository.BulkInsert(Arg.Any<IEnumerable<OfferItem>>(), CancellationToken.None).Returns(async call =>
        {
            var offerItems = (call.Arg<IEnumerable<OfferItem>>()).ToList();

            FakeDataSet.AddRange(offerItems);

            return await repository.GetByPosition(offerItems.FirstOrDefault()?.OfferPositionId?.Value ?? Guid.Empty, CancellationToken.None);
        });

        return repository;
    }
}