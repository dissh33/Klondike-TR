using AutoMapper;
using Offers.Application.Contracts;
using Offers.Application.Mapping;
using Offers.Tests.Application.Mocks;

namespace Offers.Tests.Application.Setup;

public class OfferTestsSetup
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _uow;

    public OfferTestsSetup()
    {
        var mapperConfig = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile<OfferProfile>();
            configuration.AddProfile<OfferPositionProfile>();
            configuration.AddProfile<OfferItemProfile>();
        });

        _mapper = mapperConfig.CreateMapper();

        _uow = UnitOfWorkMock.GetUnitOfWork();
    }
}
