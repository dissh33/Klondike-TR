using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.Collection;
using Items.Api.Dtos.Collection;
using Items.Api.Queries.Collection;
using Items.Application.CommandHandlers.CollectionHandlers;
using Items.Application.QueryHandlers.CollectionHandlers;
using Items.Domain.Enums;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Collections;

public class CollectionAddTests : CollectionTestsSetup
{
    private readonly CollectionAddHandler _sut;
    private readonly CollectionGetAllHandler _getAll;
    private readonly CollectionGetByIdHandler _getById;

    private readonly CollectionAddCommand _command;

    public CollectionAddTests()
    {
        _sut = new CollectionAddHandler(_uow, _mapper);
        _getAll = new CollectionGetAllHandler(_uow, _mapper);
        _getById = new CollectionGetByIdHandler(_uow, _mapper);

        _command = new CollectionAddCommand
        {
            Name = "n1",
            Status = (int)ItemStatus.Disabled,
            IconId = Guid.NewGuid(),
        };
    }

    [Fact]
    public async Task ShouldAddNewCollectionToCollectionsSet()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);
        var all = await _getAll.Handle(new CollectionGetAllQuery(), CancellationToken.None);
        
        //assert
        all.Should().HaveCount(CollectionRepositoryMock.InitialFakeDataSet.Count() + 1);
    }


    [Fact]
    public async Task ShouldAddAndReturnCollectionDto_WithValuesFromCommand() 
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionDto>();
        actual.Should().NotBeNull();

        actual!.IconId.Should().Be(_command.IconId);
        actual.Name.Should().Be(_command.Name);
        actual.Status.Should().Be(_command.Status);
    }

    [Fact]
    public async Task ShouldReturnAddedCollectionDto() 
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var added = await _getById.Handle(new CollectionGetByIdQuery(actual?.Id ?? Guid.Empty), CancellationToken.None);
        
        //assert
        actual.Should().BeOfType<CollectionDto>();
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