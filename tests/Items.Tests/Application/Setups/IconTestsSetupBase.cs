﻿using AutoMapper;
using Items.Application.Contracts;
using Items.Application.Mapping;
using Items.Tests.Application.Mocks;

namespace Items.Tests.Application.Setups;

public class IconTestsSetupBase
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _uow;

    public IconTestsSetupBase()
    {
        var mapperConfig = new MapperConfiguration(configuration => configuration.AddProfile<IconProfile>());
        _mapper = mapperConfig.CreateMapper();

        _uow = UnitOfWorkMock.GetMock();
    }
}