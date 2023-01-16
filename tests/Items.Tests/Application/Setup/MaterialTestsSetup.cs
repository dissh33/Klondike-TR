using AutoMapper;
using Items.Application.Contracts;
using Items.Application.Mapping;
using Items.Tests.Application.Mocks;

namespace Items.Tests.Application.Setup;

public class MaterialTestsSetup
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _uow;

    public MaterialTestsSetup()
    {
        var mapperConfig = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile<MaterialProfile>();
            configuration.AddProfile<IconProfile>();
        });

        _mapper = mapperConfig.CreateMapper();

        _uow = UnitOfWorkMock.GetUnitOfWork();
    }
}