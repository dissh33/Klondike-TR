using System.Data;
using System.Text;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain;
using Newtonsoft.Json;

namespace ItemManagementService.Infrastructure.Repositories;

public abstract class Repository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected const int SqlTimeout = 3600;
    protected const string SchemaName = "public";

    protected string TableName { get; }
    protected string SelectColumns { get; }
    protected string InsertColumns { get; }
    protected string UpdateColumns { get; }
    
    static Repository()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    protected Repository()
    {
        TableName = InsertUnderscoreBeforeUpperCase(typeof(T).Name);
        
        var columnsList = GetColumns().ToList();
        SelectColumns = string.Join(", ", columnsList.Select(InsertUnderscoreBeforeUpperCase));
        InsertColumns = $"({SelectColumns}) VALUES ({string.Join(", ", columnsList.Select(e => "@" + e))})";
        UpdateColumns = string.Join(", ", columnsList.Select(e => $"{InsertUnderscoreBeforeUpperCase(e)} = @{e}"));
    }

    protected CommandDefinition GetByIdCommand(Guid id, CancellationToken ct)
    {
        var sql = $"SELECT {SelectColumns} FROM {SchemaName}.{TableName} WHERE id = @id";

        return new CommandDefinition(
            sql,
            new { id },
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition GetAllCommand(CancellationToken ct)
    {
        var sql = $"SELECT {SelectColumns} FROM {SchemaName}.{TableName}";

        return new CommandDefinition(
            sql,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition InsertCommand(T entity, CancellationToken ct)
    {
        var sql = $"INSERT INTO {SchemaName}.{TableName} {InsertColumns} RETURNING id";
        
        return new CommandDefinition(
            sql,
            entity,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition UpdateCommand(T entity, CancellationToken ct)
    {
        var sql = $"UPDATE {SchemaName}.{TableName} SET {UpdateColumns} WHERE id = @id RETURNING id";

        return new CommandDefinition(
            sql,
            entity,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition DeleteCommand(Guid id, CancellationToken ct)
    {
        var sql = $"DELETE FROM {SchemaName}.{TableName} WHERE id = @id";

        return new CommandDefinition(
            sql,
            new { id },
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    private IEnumerable<string> GetColumns()
    {
        return typeof(T)
            .GetProperties()
            .Select(prop => prop.Name);
    }

    private string InsertUnderscoreBeforeUpperCase(string str)
    {
        var sb = new StringBuilder();

        char previousChar = char.MinValue;

        foreach (char c in str)
        {
            if (char.IsUpper(c))
            {
                if (sb.Length != 0 && previousChar != ' ')
                {
                    sb.Append('_');
                }
            }

            sb.Append(c);

            previousChar = c;
        }

        return sb.ToString().ToLower();
    }

    public virtual void Dispose()
    {

    }


    public class JsonObjectTypeHandler : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object? value)
        {
            parameter.Value = (value == null)
                ? DBNull.Value
                : JsonConvert.SerializeObject(value);
            parameter.DbType = DbType.String;
        }

        public object? Parse(Type destinationType, object value)
        {
            return JsonConvert.DeserializeObject(value.ToString()!, destinationType);
        }
    }
}
