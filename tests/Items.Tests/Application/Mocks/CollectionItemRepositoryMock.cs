using Items.Application.Contracts;
using NSubstitute;
using System.Threading;
using System;
using Items.Domain.Entities;
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

        repository.Update(Arg.Any<CollectionItem>(), CancellationToken.None).Returns(async call =>
        {
            var collectionItem = call.Arg<CollectionItem>();

            var itemFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collectionItem.Id);

            if (itemFromSet is null) return null;

            fakeDataSet.Remove(itemFromSet);
            fakeDataSet.Add(collectionItem);

            return await repository.GetById(collectionItem.Id, CancellationToken.None);
        });

        repository.UpdateIcon(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None).Returns(async call =>
        {
            var collectionItemId = call.ArgAt<Guid>(0);

            var materialFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collectionItemId);

            if (materialFromSet is null) return null;

            var newIconId = call.ArgAt<Guid>(1);
            materialFromSet.AddIcon(newIconId);

            return await repository.GetById(collectionItemId, CancellationToken.None);
        });

        repository.UpdateName(Arg.Any<Guid>(), Arg.Any<string>(), CancellationToken.None).Returns(async call =>
        {
            var collectionItemId = call.Arg<Guid>();
            var itemFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collectionItemId);

            if (itemFromSet is null) return null;

            var newName = call.Arg<string>();
            var updatedIcon = new CollectionItem(
                newName,
                itemFromSet.CollectionId ?? Guid.Empty,
                itemFromSet.Id,
                itemFromSet.ExternalId);

            fakeDataSet.Remove(itemFromSet);
            fakeDataSet.Add(updatedIcon);

            return await repository.GetById(updatedIcon.Id, CancellationToken.None);
        });

        repository.UpdateCollection(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None).Returns(async call =>
        {
            var collectionItemId = call.ArgAt<Guid>(0);
            var itemFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collectionItemId);

            if (itemFromSet is null) return null;

            var newCollectionId = call.ArgAt<Guid>(1);
            var updatedIcon = new CollectionItem(
                itemFromSet.Name,
                newCollectionId,
                itemFromSet.Id,
                itemFromSet.ExternalId);

            fakeDataSet.Remove(itemFromSet);
            fakeDataSet.Add(updatedIcon);

            return await repository.GetById(updatedIcon.Id, CancellationToken.None);
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