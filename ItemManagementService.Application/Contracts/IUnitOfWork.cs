
namespace ItemManagementService.Application.Contracts;

public interface IUnitOfWork 
{
    public IIconRepository? IconRepository { get; }
    public IMaterialRepository? MaterialRepository { get; }
    public ICollectionRepository? CollectionRepository { get; }
    public ICollectionItemRepository? CollectionItemRepository { get; }
}
