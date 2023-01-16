using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.CollectionItem;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Queries.CollectionItem;
using Items.Application.CommandHandlers.CollectionItemHandlers;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setup;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.CollectionItems;

public class CollectionItemAddTests : CollectionItemTestsSetup
{
    private readonly CollectionItemAddHandler _sut;
    private readonly CollectionItemGetAllHandler _getAll;
    private readonly CollectionItemGetByIdHandler _getById;

    private readonly CollectionItemAddCommand _command;

    public CollectionItemAddTests()
    {
        _sut = new CollectionItemAddHandler(_uow, _mapper);
        _getAll = new CollectionItemGetAllHandler(_uow, _mapper);
        _getById = new CollectionItemGetByIdHandler(_uow, _mapper);

        _command = new CollectionItemAddCommand
        {
            Name = "n1",
            CollectionId = Guid.NewGuid(),
            IconId = Guid.NewGuid(),
        };
    }

    [Fact]
    public async Task ShouldAddNewCollectionItemToSet()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new CollectionItemGetAllQuery(), CancellationToken.None);
        
        //assert
        all.Should().HaveCount(CollectionItemRepositoryMock.InitialFakeDataSet.Count() + 1);
    }


    [Fact]
    public async Task ShouldAddAndReturnCollectionItemDto_WithValuesFromCommand() 
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionItemDto>();
        actual.Should().NotBeNull();

        actual!.IconId.Should().Be(_command.IconId);
        actual.Name.Should().Be(_command.Name);
        actual.CollectionId.Should().Be(_command.CollectionId);
    }

    [Fact]
    public async Task ShouldReturnAddedCollectionItemDto() 
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var added = await _getById.Handle(new CollectionItemGetByIdQuery(actual?.Id ?? Guid.Empty), CancellationToken.None);
        
        //assert
        actual.Should().BeOfType<CollectionItemDto>();
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