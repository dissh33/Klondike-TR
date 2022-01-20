using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain;

namespace ItemManagementService.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseGenericRepository<T> where T : BaseEntity
{
}
