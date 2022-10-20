using Items.Application.Contracts;
using Items.Domain.Entities;
using NSubstitute;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using Items.Api.Queries.Collection;
using Items.Domain.Enums;

namespace Items.Tests.Application.Mocks;

public static class CollectionRepositoryMock
{
    public static IEnumerable<Collection> InitialFakeDataSet => SeedData();

    private static IEnumerable<Collection> SeedData()
    {
        var data = new List<Collection>();

        for (int i = 0; i < 3; i++)
        {
            var collectionItem = new Collection($"name{i}-{i}", status: ItemStatus.Disabled, id: Guid.NewGuid());
            data.Add(collectionItem);
        }

        return data;
    }

    public static ICollectionRepository GetRepository()
    {
        var repository = Substitute.For<ICollectionRepository>();

        var fakeDataSet = SeedData().ToList();

        repository.GetAll(CancellationToken.None).Returns(fakeDataSet);

        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            fakeDataSet.FirstOrDefault(fake => fake.Id == call.Arg<Guid>()));

        repository.GetByFilter(Arg.Any<CollectionGetByFilterQuery>(), CancellationToken.None).Returns(call =>
        {
            var filter = call.Arg<CollectionGetByFilterQuery>();

            return fakeDataSet.Where(fake =>
                (fake.Name is null || filter.Name is null || fake.Name.ToLower().Contains(filter.Name.ToLower())) &&
                (filter.Status is null || fake.Status == (ItemStatus)filter.Status) &&
                filter.StartDate is null && filter.EndDate is null);
        });

        repository.Insert(Arg.Any<Collection>(), CancellationToken.None).Returns(async call =>
        {
            var collection = call.Arg<Collection>();

            fakeDataSet.Add(collection);

            return await repository.GetById(collection.Id, CancellationToken.None);
        });

        repository.Update(Arg.Any<Collection>(), CancellationToken.None).Returns(async call =>
        {
            var collection = call.Arg<Collection>();

            var collectionFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collection.Id);

            if (collectionFromSet is null) return null;

            fakeDataSet.Remove(collectionFromSet);
            fakeDataSet.Add(collection);

            return await repository.GetById(collection.Id, CancellationToken.None);
        });

        repository.UpdateName(Arg.Any<Guid>(), Arg.Any<string>(), CancellationToken.None).Returns(async call =>
        {
            var collectionId = call.Arg<Guid>();
            var collectionFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collectionId);

            if (collectionFromSet is null) return null;

            var newName = call.Arg<string>();
            var updatedCollection = new Collection(
                newName,
                status: collectionFromSet.Status ?? ItemStatus.Disabled,
                id: collectionFromSet.Id,
                externalId: collectionFromSet.ExternalId);

            fakeDataSet.Remove(collectionFromSet);
            fakeDataSet.Add(updatedCollection);

            return await repository.GetById(updatedCollection.Id, CancellationToken.None);
        });

        repository.UpdateStatus(Arg.Any<Guid>(), Arg.Any<int>(), CancellationToken.None).Returns(async call =>
        {
            var collectionId = call.Arg<Guid>();
            var collectionFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collectionId);

            if (collectionFromSet is null) return null;

            var newStatus = (ItemStatus)call.Arg<int>();
            var updatedCollection = new Collection(
                collectionFromSet.Name,
                status: newStatus,
                id: collectionFromSet.Id,
                externalId: collectionFromSet.ExternalId);

            fakeDataSet.Remove(collectionFromSet);
            fakeDataSet.Add(updatedCollection);

            return await repository.GetById(updatedCollection.Id, CancellationToken.None);
        });

        repository.UpdateIcon(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None).Returns(async call =>
        {
            var collectionId = call.ArgAt<Guid>(0);

            var collectionFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collectionId);

            if (collectionFromSet is null) return null;

            var newIconId = call.ArgAt<Guid>(1);
            collectionFromSet.AddIcon(newIconId);

            return await repository.GetById(collectionId, CancellationToken.None);
        });

        repository.Delete(Arg.Any<Guid>(), CancellationToken.None).Returns(call =>
        {
            var collectionId = call.Arg<Guid>();

            var collectionFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == collectionId);

            if (collectionFromSet is not null) fakeDataSet.Remove(collectionFromSet);

            return collectionFromSet is not null ? 0 : 1;
        });

        return repository;
    }
}