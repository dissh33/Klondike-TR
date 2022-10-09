using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Materials;
using Items.Api.Queries.Material;
using Items.Application.QueryHandlers.MaterialHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Materials;

public class MaterialGetAllTests : MaterialTestsSetup
{
    private readonly MaterialGetAllHandler _sut;

    public MaterialGetAllTests()
    {
        _sut = new MaterialGetAllHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnListOfMaterialDtos_WhenAnyMaterialsExists()
    {
        //act
        var actual = (await _sut.Handle(new MaterialGetAllQuery(), CancellationToken.None)).ToList();

        //assert
        actual.Should().HaveCount(MaterialRepositoryMock.InitialFakeDataSet.Count());
        actual.Should().AllBeOfType(typeof(MaterialDto));
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new MaterialGetAllQuery(), CancellationToken.None);
        
        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}