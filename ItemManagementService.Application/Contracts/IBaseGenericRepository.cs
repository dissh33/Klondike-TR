using ItemManagementService.Domain;

namespace ItemManagementService.Application.Contracts;

public interface IBaseGenericRepository<T> where T : BaseEntity
{
    public string TableName { get; }
    public List<string> Columns { get; }
}
