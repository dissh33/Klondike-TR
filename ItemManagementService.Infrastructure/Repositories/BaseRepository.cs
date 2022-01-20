using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain;
using Microsoft.Extensions.Configuration;

namespace ItemManagementService.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseGenericRepository<T> where T : BaseEntity
{
    protected readonly string ConnectionString;

    protected const int SqlTimeout = 3600;
    protected const string SchemaName = "public";

    static BaseRepository()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    protected BaseRepository(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection");
    }
}
