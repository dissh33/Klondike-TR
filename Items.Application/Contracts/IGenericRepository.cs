using Items.Domain.Entities;

namespace Items.Application.Contracts;

public interface IGenericRepository<T> : IDisposable 
    where T : BaseEntity
{
}
