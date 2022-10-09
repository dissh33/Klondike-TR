using Items.Application.Contracts;
using NSubstitute;
using System.Threading;
using System;
using Items.Domain.Entities;
using Items.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Items.Tests.Application.Mocks;

public static class CollectionItemRepositoryMock
{
    public static IEnumerable<CollectionItem> InitialFakeDataSet => SeedData();

    private static IEnumerable<CollectionItem> SeedData()
    {
        var data = new List<CollectionItem>();

        foreach (var collection in CollectionRepositoryMock.InitialFakeDataSet)
        {
            for (int i = 0; i < 5; i++)
            {
                var collectionItem = new CollectionItem($"name{i}-{i}", collection.Id, Guid.NewGuid());
                data.Add(collectionItem);
            }
        }

        return data;
    }

    public static ICollectionItemRepository GetRepository()
    {
        var repository = Substitute.For<ICollectionItemRepository>();

        var fakeDataSet = SeedData().ToList();

        repository.GetAll(CancellationToken.None).Returns(fakeDataSet);

        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            fakeDataSet.FirstOrDefault(fake => fake.Id == call.Arg<Guid>()));

        repository.Insert(Arg.Any<CollectionItem>(), CancellationToken.None).Returns(async call =>
        {
            var collectionItem = call.Arg<CollectionItem>();

            fakeDataSet.Add(collectionItem);

            return await repository.GetById(collectionItem.Id, CancellationToken.None);
        });

        repository.Delete(Arg.Any<Guid>(), CancellationToken.None).Returns(call =>
        {
            var id = call.Arg<Guid>();

            var iconFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == id);

            if (iconFromSet is not null) fakeDataSet.Remove(iconFromSet);

            return iconFromSet is not null ? 0 : 1;
        });

        return repository;
    }
}