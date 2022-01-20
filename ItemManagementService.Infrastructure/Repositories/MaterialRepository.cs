using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;

namespace ItemManagementService.Infrastructure.Repositories;

public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
{
}
