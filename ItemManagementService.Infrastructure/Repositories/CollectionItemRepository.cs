using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionItemRepository : BaseRepository<CollectionItem>, ICollectionItemRepository
{
    private readonly NpgsqlConnection _connection;

    public CollectionItemRepository(IConfiguration configuration) : base(configuration)
    {
        _connection = new NpgsqlConnection(ConnectionString);
    }
}
