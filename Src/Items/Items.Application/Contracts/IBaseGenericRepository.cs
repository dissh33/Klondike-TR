using Items.Domain.Entities;

namespace Items.Application.Contracts;

public interface IBaseGenericRepository<T> : IDisposable 
    where T : BaseEntity
{
}
