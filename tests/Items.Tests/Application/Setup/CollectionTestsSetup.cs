using AutoMapper;
using Items.Application.Contracts;
using Items.Application.Mapping;
using Items.Tests.Application.Mocks;

namespace Items.Tests.Application.Setup;

public class CollectionTestsSetup
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _uow;

    public CollectionTestsSetup()
    {
        var mapperConfig = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile<CollectionProfile>();
            configuration.AddProfile<CollectionItemProfile>();
            configuration.AddProfile<IconProfile>();
        });

        _mapper = mapperConfig.CreateMapper();

        _uow = UnitOfWorkMock.GetUnitOfWork();
    }
}