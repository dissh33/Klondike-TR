using System.Reflection;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain;

namespace ItemManagementService.Infrastructure.Repositories;

public abstract class Repository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected const int SqlTimeout = 3600;
    protected const string SchemaName = "public";

    protected string TableName { get; }
    protected List<string> Columns { get; }
    

    static Repository()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    protected Repository()
    {
        TableName = typeof(T).Name.ToLower();
        Columns = GetColumns();
    }

    private List<string> GetColumns()
    {
        return typeof(T)
            .GetProperties()
            .Where(prop => prop.Name != "Id" && !prop.PropertyType.GetTypeInfo().IsGenericType)
            .Select(prop => prop.Name)
            .ToList();
    }

    protected CommandDefinition GetByIdCommand(int id, CancellationToken ct)
    {
        var sql = $"SELECT {Columns} FROM {SchemaName}.{TableName} WHERE id = @id";

        return new CommandDefinition(
            sql,
            new { id },
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition GetAllCommand(CancellationToken ct)
    {
        var sql = $"SELECT {Columns} FROM {SchemaName}.{TableName}";

        return new CommandDefinition(
            sql,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition InsertCommand(T entity, CancellationToken ct)
    {
        var columnsString = string.Join(", ", Columns);
        var parametersString = string.Join(", ", Columns.Select(e => "@" + e));

        var sql = $"INSERT INTO {SchemaName}.{TableName} ({columnsString}) VALUES({parametersString}) RETURNING id";

        return new CommandDefinition(
            sql,
            entity,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition UpdateCommand(T entity, CancellationToken ct)
    {
        var setString = string.Join(", ", Columns.Select(e => $"{e} = @{e}"));

        var sql = $"UPDATE {SchemaName}.{TableName} SET {setString} WHERE id = @id RETURNING id";

        return new CommandDefinition(
            sql,
            entity,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition DeleteCommand(int id, CancellationToken ct)
    {
        var sql = $"DELETE FROM {SchemaName}.{TableName} WHERE id = @id";

        return new CommandDefinition(
            sql,
            new { id },
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    public virtual void Dispose()
    {
        
    }
}
