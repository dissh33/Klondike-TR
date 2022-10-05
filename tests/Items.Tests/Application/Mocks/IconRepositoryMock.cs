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
    public static IEnumerable<Icon> FakeData => SeedData();

    private static IEnumerable<Icon> SeedData()
    {
        var data = new List<Icon>();

        for (int i = 0; i < 5; i++)
        {
            var icon = new Icon($"t{i}", Array.Empty<byte>(), $"f{i}", Guid.NewGuid());
            data.Add(icon);
        }

        return data;
    }

    public static IIconRepository GetRepository()
    {
        var repository = Substitute.For<IIconRepository>();

        repository.GetAll(CancellationToken.None).Returns(FakeData);

        repository.GetById(Arg.Any<Guid>(), CancellationToken.None)!
            .Returns(call => FakeData.FirstOrDefault(icon => icon.Id == call.Arg<Guid>()));

        return repository;
    }
}