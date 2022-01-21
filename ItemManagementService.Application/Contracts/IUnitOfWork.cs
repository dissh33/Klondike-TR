using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.Contracts;

public interface IUnitOfWork 
{
    IGenericRepository<Icon>? IconRepository { get; }
    IGenericRepository<Material>? MaterialRepository { get; }
    IGenericRepository<Collection>? CollectionRepository { get; }
    IGenericRepository<CollectionItem>? CollectionItemRepository { get; }
}
