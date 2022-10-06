using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Items.Domain.Enums;
using NSubstitute;

namespace Items.Tests.Application.Mocks;

public static class MaterialRepositoryMock
{
    public static IEnumerable<Material> InitialFakeDataSet => SeedData();

    private static IEnumerable<Material> SeedData()
    {
        var data = new List<Material>();

        for (int i = 0; i < 3; i++)
        {
            var material = new Material($"n{i}", MaterialType.Default, ItemStatus.Disabled, Guid.NewGuid());
            data.Add(material);
        }

        return data;
    }

    public static IMaterialRepository GetRepository()
    {
        var repository = Substitute.For<IMaterialRepository>();

        var fakeDataSet = SeedData().ToList();

        repository.GetAll(CancellationToken.None).Returns(fakeDataSet);

        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call => 
            fakeDataSet.FirstOrDefault(material => material.Id == call.Arg<Guid>()));

        repository.Insert(Arg.Any<Material>(), CancellationToken.None).Returns(async call =>
        {
            var material = call.Arg<Material>();
            
            fakeDataSet.Add(material);

            return await repository.GetById(material.Id, CancellationToken.None);
        });

        repository.Update(Arg.Any<Material>(), CancellationToken.None).Returns(async call =>
        {
            var material = call.Arg<Material>();

            var materialFromSet = fakeDataSet.First(fake => fake.Id == material.Id);

            fakeDataSet.Remove(materialFromSet);
            fakeDataSet.Add(material);

            return await repository.GetById(material.Id, CancellationToken.None);
        });

        repository.UpdateStatus(Arg.Any<Guid>(), Arg.Any<int>(), CancellationToken.None).Returns(async call =>
        {
            var materialId = call.Arg<Guid>();
            var materialFromSet = fakeDataSet.First(fake => fake.Id == materialId);

            var newStatus = (ItemStatus) call.Arg<int>();
            var updatedMaterial = new Material(
                materialFromSet.Name, 
                materialFromSet.Type, 
                newStatus,
                materialFromSet.Id, 
                materialFromSet.ExternalId);

            fakeDataSet.Remove(materialFromSet);
            fakeDataSet.Add(updatedMaterial);

            return await repository.GetById(updatedMaterial.Id, CancellationToken.None);
        });

        repository.UpdateIcon(Arg.Any<Guid>(), Arg.Any<Guid>(), CancellationToken.None).Returns(async call =>
        {
            var materialId = call.ArgAt<Guid>(0);

            var materialFromSet = fakeDataSet.First(fake => fake.Id == materialId);

            var newIconId = call.ArgAt<Guid>(1);
            materialFromSet.AddIcon(newIconId);

            return await repository.GetById(materialId, CancellationToken.None);
        });

        return repository;
    }
}