using NSubstitute;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using System.Linq;
using System.Runtime.CompilerServices;

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

        repository.GetCount(CancellationToken.None)!.Returns(call => FakeDataSet.Count);

        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            FakeDataSet.FirstOrDefault(fake => fake.Id.Value == call.Arg<Guid>()));

        repository.GetByPage(Arg.Any<int>(), Arg.Any<int>(), CancellationToken.None, Arg.Any<Dictionary<string, string?>?>())
            .Returns(call =>
            {
                var page = call.ArgAt<int>(0);
                var pageSize = call.ArgAt<int>(1);
                var orderBy = call.Arg<Dictionary<string, string?>?>();

                if (orderBy is not null && orderBy.ContainsKey("title"))
                {
                    return FakeDataSet.OrderBy(offer => offer.Title).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (orderBy is not null && orderBy.ContainsKey("type"))
                {
                    return FakeDataSet.OrderBy(offer => offer.Type).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (orderBy is not null && orderBy.ContainsKey("status"))
                {
                    return FakeDataSet.OrderBy(offer => offer.Status).Skip((page - 1) * pageSize).Take(pageSize);
                }

                return FakeDataSet.Skip((page -1) * pageSize).Take(pageSize);
            });
            

        repository.Insert(Arg.Any<Offer>(), CancellationToken.None).Returns(async call =>
        {
            var offer = call.Arg<Offer>();

            var newOffer = new Offer(
                offer.Title,
                offer.Message,
                offer.Expression,
                offer.Type, 
                offer.Status,
                offer.Id.Value);

            FakeDataSet.Add(newOffer);

            return await repository.GetById(newOffer.Id.Value, CancellationToken.None);
        });

        return repository;
    }
}
