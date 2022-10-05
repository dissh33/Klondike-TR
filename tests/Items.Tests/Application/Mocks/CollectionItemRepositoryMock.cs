using Items.Application.Contracts;
using NSubstitute;

namespace Items.Tests.Application.Mocks;

public static class CollectionItemRepositoryMock
{
    public static ICollectionItemRepository GetRepository()
    {
        var repository = Substitute.For<ICollectionItemRepository>();

        return repository;
    }
}