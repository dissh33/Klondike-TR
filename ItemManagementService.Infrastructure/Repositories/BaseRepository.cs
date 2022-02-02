using System.Data;
using System.Text;
using Dapper;
using ItemManagementService.Application.Contracts;
using ItemManagementService.Domain;
using Newtonsoft.Json;
using Serilog;

namespace ItemManagementService.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected const int SqlTimeout = 3600;
    protected const string SchemaName = "public";

    protected IDbTransaction Transaction { get; }
    protected ILogger Logger { get; }
    
    protected IDbConnection? Connection { get; }

    protected string TableName { get; }

    static BaseRepository()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    protected BaseRepository(IDbTransaction transaction, ILogger logger)
    {
        Transaction = transaction;
        Logger = logger;

        Connection = Transaction.Connection;

        TableName = InsertUnderscoreBeforeUpperCase(typeof(T).Name);
    }

    protected CommandDefinition GetByIdBaseCommand(Guid id, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var sql = $"SELECT {selectColumns} FROM {SchemaName}.{TableName} WHERE id = @id";

        return new CommandDefinition(
            commandText: sql,
            parameters: new { id },
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition GetAllBaseCommand(CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var sql = $"SELECT {selectColumns} FROM {SchemaName}.{TableName}";

        return new CommandDefinition(
            commandText: sql,
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition InsertBaseCommand(T entity, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var insertColumns = $"({selectColumns}) VALUES ({string.Join(", ", GetColumns().Select(e => "@" + e))})";

        var sql = $"INSERT INTO {SchemaName}.{TableName} {insertColumns} RETURNING id";
        
        return new CommandDefinition(
            commandText: sql,
            parameters: entity,
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition UpdateBaseCommand(T entity, CancellationToken ct)
    {
        var updateColumns = string.Join(", ", GetColumns().Select(e => $"{InsertUnderscoreBeforeUpperCase(e)} = @{e}"));

        var sql = $"UPDATE {SchemaName}.{TableName} SET {updateColumns} WHERE id = @id RETURNING id";

        return new CommandDefinition(
            commandText: sql,
            parameters: entity,
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected CommandDefinition DeleteBaseCommand(Guid id, CancellationToken ct)
    {
        var sql = $"DELETE FROM {SchemaName}.{TableName} WHERE id = @id";

        return new CommandDefinition(
            commandText: sql,
            parameters: new { id },
            transaction: Transaction,
            commandTimeout: SqlTimeout,
            cancellationToken: ct);
    }

    protected IEnumerable<string> GetColumns()
    {
        return typeof(T)
            .GetProperties()
            .Where(prop => prop.Name != "DomainEvents")
            .Select(prop => prop.Name);
    }

    protected string InsertUnderscoreBeforeUpperCase(string str)
    {
        var sb = new StringBuilder();

        char previousChar = char.MinValue;

        foreach (char ch in str)
        {
            if (char.IsUpper(ch))
            {
                if (sb.Length != 0 && previousChar != ' ')
                {
                    sb.Append('_');
                }
            }

            sb.Append(ch);

            previousChar = ch;
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
