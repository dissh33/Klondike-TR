using Items.Application.Contracts;
using NSubstitute;

namespace Items.Tests.Application.Mocks;

public static class CollectionRepositoryMock
{
    public static ICollectionRepository GetRepository()
    {
        var repository = Substitute.For<ICollectionRepository>();

        return repository;
    }
}