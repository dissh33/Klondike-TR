using ItemManagementService.Domain;

namespace ItemManagementService.Application.Contracts;

public interface IGenericRepository<T> : IDisposable 
    where T : BaseEntity
{
}
