using NSubstitute;
using Offers.Application.Contracts;

namespace Offers.Tests.Application.Mocks;

public static class UnitOfWorkMock
{
    public static IUnitOfWork GetUnitOfWork()
    {
        var uow = Substitute.For<IUnitOfWork>();

        var offerRepository = OfferRepositoryMock.GetRepository();
        var positionRepository = OfferPositionRepositoryMock.GetRepository();
        var offerItemRepository = OfferItemRepositoryMock.GetRepository();

        uow.OfferRepository.Returns(offerRepository);
        uow.OfferPositionRepository.Returns(positionRepository);
        uow.OfferItemRepository.Returns(offerItemRepository);

        return uow;
    }
}