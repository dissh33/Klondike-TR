using System.Data;

namespace Items.Application.Contracts;

public interface IUnitOfWork 
{
    IIconRepository IconRepository { get; }
    IMaterialRepository MaterialRepository { get; }
    ICollectionRepository CollectionRepository { get; }
    ICollectionItemRepository CollectionItemRepository { get; }

    IDbConnection CreateConnection();

    public void Commit();
    public void Rollback();
}
