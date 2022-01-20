using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ItemManagementService.Infrastructure.Repositories;

public class CollectionRepository : BaseRepository<Collection>, ICollectionRepository
{
    private readonly NpgsqlConnection _connection;

    public CollectionRepository(IConfiguration configuration) : base(configuration)
    {
        _connection = new NpgsqlConnection(ConnectionString);
    }
}
