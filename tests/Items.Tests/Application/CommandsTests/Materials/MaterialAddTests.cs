using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.Material;
using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.CommandHandlers.MaterialHandlers;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Domain.Enums;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Materials;

public class IconAddTests : MaterialTestsSetup
{
    private readonly MaterialAddHandler _sut;
    private readonly MaterialGetAllHandler _getAll;
    private readonly MaterialGetByIdHandler _getById;

    private readonly MaterialAddCommand _command;

    public IconAddTests()
    {
        _sut = new MaterialAddHandler(_uow, _mapper);
        _getAll = new MaterialGetAllHandler(_uow, _mapper);
        _getById = new MaterialGetByIdHandler(_uow, _mapper);

        _command = new MaterialAddCommand
        {
            Name = "n1",
            Type = (int)MaterialType.Specific,
            Status = (int)ItemStatus.Disabled,
            IconId = Guid.NewGuid(),
        };
    }

    [Fact]
    public async Task ShouldAddNewMaterialToMaterialsSet()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None);
        
        //assert
        all.Should().HaveCount(MaterialRepositoryMock.InitialFakeDataSet.Count() + 1);
    }


    [Fact]
    public async Task ShouldAddAndReturnMaterialDto_WithValuesFromCommand() 
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<MaterialDto>();
        actual.Should().NotBeNull();

        actual!.IconId.Should().Be(_command.IconId);
        actual.Name.Should().Be(_command.Name);
        actual.Type.Should().Be(_command.Type);
        actual.Status.Should().Be(_command.Status);
    }

    [Fact]
    public async Task ShouldReturnAddedMaterialDto() 
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var added = await _getById.Handle(new MaterialGetByIdQuery(actual?.Id ?? Guid.Empty), CancellationToken.None);
        
        //assert
        actual.Should().BeOfType<MaterialDto>();
        actual.Should().Be(added);
    }

    [Fact]
    public async Task ShouldCommitOnce()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        
        //assert
        _uow.Received(1).Commit();
    }
}