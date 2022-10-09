using Items.Application.Contracts;
using Items.Domain.Entities;
using NSubstitute;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
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

        return repository;
    }
}