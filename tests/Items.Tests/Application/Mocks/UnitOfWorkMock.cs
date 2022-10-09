using Items.Application.Contracts;
using NSubstitute;

namespace Items.Tests.Application.Mocks;

public static class UnitOfWorkMock
{
    public static IUnitOfWork GetUnitOfWork()
    {
        var uow = Substitute.For<IUnitOfWork>();

        var iconRepositoryMock = IconRepositoryMock.GetRepository();
        var materialRepositoryMock = MaterialRepositoryMock.GetRepository();
        var collectionRepositoryMock = CollectionRepositoryMock.GetRepository();
        var collectionItemRepositoryMock = CollectionItemRepositoryMock.GetRepository();

        uow.IconRepository.Returns(iconRepositoryMock);
        uow.MaterialRepository.Returns(materialRepositoryMock);
        uow.CollectionRepository.Returns(collectionRepositoryMock);
        uow.CollectionItemRepository.Returns(collectionItemRepositoryMock);

        return uow;
    }
}