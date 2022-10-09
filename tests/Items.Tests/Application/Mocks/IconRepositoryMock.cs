using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Items.Application.Contracts;
using Items.Domain.Entities;
using NSubstitute;

namespace Items.Tests.Application.Mocks;

internal static class IconRepositoryMock
{
    public static IEnumerable<Icon> InitialFakeDataSet => SeedData();

    private static IEnumerable<Icon> SeedData()
    {
        var data = new List<Icon>();

        for (int i = 0; i < 5; i++)
        {
            var icon = new Icon($"title{i}-{i}", Array.Empty<byte>(), $"file-name{i}-{i}", Guid.NewGuid());
            data.Add(icon);
        }

        return data;
    }

    public static IIconRepository GetRepository()
    {
        var repository = Substitute.For<IIconRepository>();

        var fakeDataSet = SeedData().ToList();

        repository.GetAll(CancellationToken.None).Returns(fakeDataSet);

        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!.Returns(call =>
            fakeDataSet.FirstOrDefault(fake => fake.Id == call.Arg<Guid>()));

        repository.Insert(Arg.Any<Icon>(), CancellationToken.None).Returns(async call =>
        {
            var icon = call.Arg<Icon>();

            fakeDataSet.Add(icon);

            return await repository.GetById(icon.Id, CancellationToken.None);
        });

        repository.Update(Arg.Any<Icon>(), CancellationToken.None).Returns(async call =>
        {
            var icon = call.Arg<Icon>();

            var iconFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == icon.Id);

            if (iconFromSet is null) return null;

            fakeDataSet.Remove(iconFromSet);
            fakeDataSet.Add(icon);

            return await repository.GetById(icon.Id, CancellationToken.None);
        });

        repository.UpdateTitle(Arg.Any<Guid>(), Arg.Any<string>(), CancellationToken.None).Returns(async call =>
        {
            var iconId = call.Arg<Guid>();
            var iconFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == iconId);

            if (iconFromSet is null) return null;

            var newTitle = call.Arg<string>();
            var updatedIcon = new Icon(
                newTitle,
                iconFromSet.FileBinary,
                iconFromSet.FileName,
                iconFromSet.Id,
                iconFromSet.ExternalId);

            fakeDataSet.Remove(iconFromSet);
            fakeDataSet.Add(updatedIcon);

            return await repository.GetById(updatedIcon.Id, CancellationToken.None);
        });

        repository.UpdateFile(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<byte[]>(), CancellationToken.None).Returns(async call =>
        {
            var iconId = call.Arg<Guid>();
            var iconFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == iconId);

            if (iconFromSet is null) return null;

            var newFileName = call.Arg<string>();
            var newFileBinary = call.Arg<byte[]>();
            var updatedIcon = new Icon(
                iconFromSet.Title,
                newFileBinary,
                newFileName,
                iconFromSet.Id,
                iconFromSet.ExternalId);

            fakeDataSet.Remove(iconFromSet);
            fakeDataSet.Add(updatedIcon);

            return await repository.GetById(updatedIcon.Id, CancellationToken.None);
        });

        repository.Delete(Arg.Any<Guid>(), CancellationToken.None).Returns(call =>
        {
            var iconId = call.Arg<Guid>();

            var iconFromSet = fakeDataSet.FirstOrDefault(fake => fake.Id == iconId);

            if (iconFromSet is not null) fakeDataSet.Remove(iconFromSet);

            return iconFromSet is not null ? 0 : 1;
        });

        return repository;
    }
}