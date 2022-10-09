using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.Icon;
using Items.Application.QueryHandlers.IconHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.QueriesTests.Icons;

public class IconGetAllTests : IconTestsSetup
{
    private readonly IconGetAllHandler _sut;

    public IconGetAllTests()
    {
        _sut = new IconGetAllHandler(_uow, _mapper);
    }

    [Fact]
    public async Task ShouldReturnListOfIconDtos_WhenAnyIconsExists()
    {
        //act
        var actual = (await _sut.Handle(new IconGetAllQuery(), CancellationToken.None)).ToList();

        //assert
        actual.Should().HaveCount(IconRepositoryMock.InitialFakeDataSet.Count());
        actual.Should().AllBeOfType(typeof(IconDto));
    }

    [Fact]
    public async Task ShouldNotCommit()
    {
        //act
        await _sut.Handle(new IconGetAllQuery(), CancellationToken.None);
        
        //assert
        _uow.DidNotReceiveWithAnyArgs().Commit();
    }
}