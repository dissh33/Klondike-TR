using AutoMapper;
using Items.Application.Contracts;
using Items.Application.Mapping;
using Items.Tests.Application.Mocks;

namespace Items.Tests.Application.Setups;

public class IconTestsSetup
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _uow;

    public IconTestsSetup()
    {
        var mapperConfig = new MapperConfiguration(configuration => configuration.AddProfile<IconProfile>());
        _mapper = mapperConfig.CreateMapper();

        _uow = UnitOfWorkMock.GetUnitOfWork();
    }
}