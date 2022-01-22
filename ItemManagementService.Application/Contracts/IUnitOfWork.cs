using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Application.Contracts;

public interface IUnitOfWork 
{
    IIconRepository? IconRepository { get; }
    IMaterialRepository? MaterialRepository { get; }
    ICollectionRepository? CollectionRepository { get; }
    ICollectionItemRepository? CollectionItemRepository { get; }
}
