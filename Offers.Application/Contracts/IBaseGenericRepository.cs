using Offers.Domain.SeedWork;

namespace Offers.Application.Contracts;

public interface IBaseGenericRepository<T> : IDisposable
    where T : BaseEntity
{
}
