using Items.Api.Commands.Material;
using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.CommandHandlers.MaterialHandlers;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Tests.Application.Mocks;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using FluentAssertions;
using Items.Domain.Enums;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.Commands.Materials;

public class MaterialUpdateStatusTests : MaterialTestsSetupBase
{
    private readonly MaterialUpdateStatusHandler _sut;
    private readonly MaterialGetAllHandler _getAll;
    private readonly MaterialGetByIdHandler _getById;

    private readonly MaterialUpdateStatusCommand _command;

    public MaterialUpdateStatusTests()
    {
        _sut = new MaterialUpdateStatusHandler(_uow, _mapper);
        _getAll = new MaterialGetAllHandler(_uow, _mapper);
        _getById = new MaterialGetByIdHandler(_uow, _mapper);

        var fakeId = _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None).GetAwaiter().GetResult().First().Id;

        _command = new MaterialUpdateStatusCommand()
        {
            Id = fakeId,
            Status = (int) ItemStatus.Removed,
        };
    }

    [Fact]
    public async Task ShouldUpdateAndReturnMaterialDto_WithNewIconButSameRestValues()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<MaterialDto>();

        actual.Status.Should().Be(_command.Status);
        actual.Id.Should().Be(_command.Id);

        actual.IconId.Should().Be(MaterialRepositoryMock.InitialFakeDataSet.First().IconId);
        actual.Name.Should().Be(MaterialRepositoryMock.InitialFakeDataSet.First().Name);
        actual.Type.Should().Be((int)MaterialRepositoryMock.InitialFakeDataSet.First().Type);
    }

    [Fact]
    public async Task ShouldReturnUpdatedMaterialDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var updated = await _getById.Handle(new MaterialGetByIdQuery(actual.Id), CancellationToken.None);

        //assert
        actual.Should().BeOfType<MaterialDto>();
        actual.Id.Should().Be(updated.Id);
    }

    [Fact]
    public async Task ShouldUpdateExistedMaterial()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new MaterialGetAllQuery(), CancellationToken.None);

        //assert
        all.Should().HaveCount(MaterialRepositoryMock.InitialFakeDataSet.Count()-1);
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