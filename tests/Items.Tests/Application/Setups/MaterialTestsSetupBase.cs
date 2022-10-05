using AutoMapper;
using Items.Application.Contracts;
using Items.Application.Mapping;
using Items.Tests.Application.Mocks;

namespace Items.Tests.Application.Setups;

public class MaterialTestsSetupBase
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _uow;

    public MaterialTestsSetupBase()
    {
        var mapperConfig = new MapperConfiguration(configuration => configuration.AddProfile<MaterialProfile>());
        _mapper = mapperConfig.CreateMapper();

        _uow = UnitOfWorkMock.GetMock();
    }
}