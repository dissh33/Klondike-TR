using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Items.Api.Commands.Collection;
using Items.Api.Dtos.Collection;
using Items.Api.Dtos.CollectionItem;
using Items.Api.Dtos.Icon;
using Items.Api.Queries.Collection;
using Items.Api.Queries.CollectionItem;
using Items.Api.Queries.Icon;
using Items.Application.CommandHandlers.CollectionHandlers;
using Items.Application.QueryHandlers.CollectionHandlers;
using Items.Application.QueryHandlers.CollectionItemHandlers;
using Items.Application.QueryHandlers.IconHandlers;
using Items.Domain;
using Items.Domain.Entities;
using Items.Domain.Enums;
using Items.Domain.Exceptions;
using Items.Tests.Application.Mocks;
using Items.Tests.Application.Setups;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Items.Tests.Application.CommandsTests.Collections;

public class CollectionConstructTests : CollectionTestsSetup
{
    private readonly CollectionConstructHandler _sut;
    private readonly CollectionGetByIdHandler _getById;
    private readonly CollectionGetAllHandler _getAllCollections;
    private readonly IconGetAllHandler _getAllIcons;
    private readonly CollectionItemGetAllHandler _getAllItems;

    private readonly Collection _collection;

    private readonly CollectionConstructCommand _command;

    public CollectionConstructTests()
    {
        _sut = new CollectionConstructHandler(_uow, _mapper);
        _getById = new CollectionGetByIdHandler(_uow, _mapper);
        _getAllCollections = new CollectionGetAllHandler(_uow, _mapper);
        _getAllIcons = new IconGetAllHandler(_uow, _mapper);
        _getAllItems = new CollectionItemGetAllHandler(_uow, _mapper);

        _collection = new Collection("name-1", status: ItemStatus.Disabled, id: Guid.NewGuid());

        _command = new CollectionConstructCommand
        {
            Name = _collection.Name,
            Status = (int) _collection.Status!,
            Icon = new IconAddDto
            {
                Title = "title-1",
                FileBinary = Array.Empty<byte>(),
                FileName = "file-name-1",
            },
            Items = new []
            {
                new CollectionItemAddDto
                {
                    Name = "item-1",
                    Icon = new IconAddDto
                    {
                        Title = "item-title-1",
                        FileBinary = Array.Empty<byte>(),
                        FileName = "item-file-name-1",
                    },
                },
                new CollectionItemAddDto
                {
                    Name = "item-2",
                    Icon = new IconAddDto
                    {
                        Title = "item-title-2",
                        FileBinary = Array.Empty<byte>(),
                        FileName = "item-file-name-2",
                    },
                },
                new CollectionItemAddDto
                {
                    Name = "item-3",
                    Icon = new IconAddDto
                    {
                        Title = "item-title-3",
                        FileBinary = Array.Empty<byte>(),
                        FileName = "item-file-name-3",
                    },
                },
                new CollectionItemAddDto
                {
                    Name = "item-4",
                    Icon = new IconAddDto
                    {
                        Title = "item-title-4",
                        FileBinary = Array.Empty<byte>(),
                        FileName = "item-file-name-4",
                    },
                },
                new CollectionItemAddDto
                {
                    Name = "item-5",
                    Icon = new IconAddDto
                    {
                        Title = "item-title-5",
                        FileBinary = Array.Empty<byte>(),
                        FileName = "item-file-name-5",
                    },
                },
            },
        };
    }

    [Fact]
    public async Task ShouldAddNewEntitiesToTheirSets()
    {
        //act
        await _sut.Handle(_command, CancellationToken.None);

        var allCollections = await _getAllCollections.Handle(new CollectionGetAllQuery(), CancellationToken.None);
        var allIcons = await _getAllIcons.Handle(new IconGetAllQuery(), CancellationToken.None);
        var allItems = await _getAllItems.Handle(new CollectionItemGetAllQuery(), CancellationToken.None);
        
        //assert
        allCollections.Should().HaveCount(CollectionRepositoryMock.InitialFakeDataSet.Count() + 1);
        allIcons.Should().HaveCount(IconRepositoryMock.InitialFakeDataSet.Count() + Constants.COLLECTION_ITEM_NUMBER + 1); //+1 for collection entity
        allItems.Should().HaveCount(CollectionItemRepositoryMock.InitialFakeDataSet.Count() + Constants.COLLECTION_ITEM_NUMBER);
    }
    
    [Fact]
    public async Task ShouldReturnCollectionFullDto_WithValuesFromCommand() 
    {
        //arrange
        _uow.CollectionRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).Returns(_collection);

        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionFullDto>();
        actual.Should().NotBeNull();

        actual!.Should().BeEquivalentTo(_command, options => 
            options.Excluding(command => command.Icon.FileBinary).Excluding(command => command.Items));

        actual!.Items.Should().BeEquivalentTo(_command.Items, options =>
            options.Excluding(item => item.Icon.FileBinary));
    }


    [Fact]
    public async Task ShouldReturnAddedCollectionFullDto()
    {
        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);
        var added = await _getById.Handle(new CollectionGetByIdQuery(actual?.Id ?? Guid.Empty), CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionFullDto>();
        added.Should().NotBeNull();

        added.Should().BeEquivalentTo(actual, options =>
            options.Excluding(dto => dto!.Icon).Excluding(dto => dto!.Items));
    }

    [Fact]
    public async Task ShouldRollbackAndReturnNull_WhenCantAddCollectionEntity()
    {
        //arrange
        _uow.CollectionRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).ReturnsNull();
        _uow.CollectionRepository.Insert(Arg.Any<Collection>(), CancellationToken.None).ReturnsNull();

        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        _uow.Received(1).Rollback();
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ShouldReturnCollectionFullDto_WithEmptyIcons_WhenIconsCantBeAdded()
    {
        //arrange
        _uow.CollectionRepository.GetById(Arg.Any<Guid>(), CancellationToken.None).Returns(_collection);
        _uow.CollectionItemRepository.GetByCollection(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(_command.Items.Select(dto => new CollectionItem(dto.Name, Guid.NewGuid())));

        _uow.IconRepository.Insert(Arg.Any<Icon>(), CancellationToken.None).ReturnsNull();
        _uow.IconRepository.BulkInsert(Arg.Any<IEnumerable<Icon>>(), CancellationToken.None).Returns(new List<Icon>());

        //act
        var actual = await _sut.Handle(_command, CancellationToken.None);

        //assert
        actual.Should().BeOfType<CollectionFullDto>();
        actual.Should().NotBeNull();

        actual!.Icon.Should().BeNull();
        actual.Should().BeEquivalentTo(_command, options =>
            options.Excluding(command => command.Icon).Excluding(command => command.Items));

        actual.Items.Should().BeEquivalentTo(_command.Items, options => options.Excluding(item => item.Icon));
        actual.Items!.Select(item => item.Icon).Should().BeEquivalentTo(new List<IconDto?> {null, null, null, null, null});
    }

    [Fact]
    public async Task ShouldThrowExceptionAndRollback_WhenCountOfAddedCollectionItemsIsNotFive()
    {
        //arrange
        _uow.CollectionItemRepository.GetByCollection(Arg.Any<Guid>(), CancellationToken.None).Returns(Enumerable.Empty<CollectionItem>());

        //act
        var act = () => _sut.Handle(_command, CancellationToken.None);

        //assert
        await act.Should().ThrowAsync<WrongCollectionItemsNumberException>();
        _uow.Received(1).Rollback();
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