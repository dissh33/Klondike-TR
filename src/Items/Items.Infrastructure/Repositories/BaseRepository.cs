using System.Data;
using System.Text;
using App.Metrics;
using Dapper;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Newtonsoft.Json;
using Serilog;

namespace Items.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseGenericRepository<T> where T : BaseEntity
{
    protected const int SQL_TIMEOUT = 3600;
    protected const string SCHEMA_NAME = "public";

    public IDbTransaction Transaction { get; }
    public ILogger Logger { get; }
    public IMetrics Metrics { get; }
    
    public IDbConnection? Connection { get; }

    protected string TableName { get; }

    static BaseRepository()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    protected BaseRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics)
    {
        Transaction = transaction;
        Logger = logger;
        Metrics = metrics;

        Connection = Transaction.Connection;

        TableName = InsertUnderscoreBeforeUpperCase(typeof(T).Name);
    }

    protected List<string> ExcludeProperties = new() { "Icon", "Items", "DomainEvents", "ExternalId" };

    protected IEnumerable<string> GetColumns()
    {
        return typeof(T)
            .GetProperties()
            .Where(prop => !ExcludeProperties.Contains(prop.Name))
            .Select(prop => prop.Name);
    }

    protected CommandDefinition GetByIdBaseCommand(Guid id, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var sql = $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName} WHERE id = @id";

        return new CommandDefinition(
            commandText: sql,
            parameters: new { id },
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);
    }

    protected CommandDefinition GetAllBaseCommand(CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));

        var sql = $"SELECT {selectColumns} FROM {SCHEMA_NAME}.{TableName}";

        return new CommandDefinition(
            commandText: sql,
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);
    }

    protected CommandDefinition InsertBaseCommand(T entity, CancellationToken ct)
    {
        var selectColumns = string.Join(", ", GetColumns().Select(InsertUnderscoreBeforeUpperCase));
        var insertColumns = $"({selectColumns}) VALUES ({string.Join(", ", GetColumns().Select(e => "@" + e))})";

        var sql = $"INSERT INTO {SCHEMA_NAME}.{TableName} {insertColumns} RETURNING id";
        
        return new CommandDefinition(
            commandText: sql,
            parameters: entity,
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);
    }

    protected CommandDefinition UpdateBaseCommand(T entity, CancellationToken ct)
    {
        var updateColumns = string.Join(", ", GetColumns().Select(e => $"{InsertUnderscoreBeforeUpperCase(e)} = @{e}"));

        var sql = $"UPDATE {SCHEMA_NAME}.{TableName} SET {updateColumns} WHERE id = @id RETURNING id";

        return new CommandDefinition(
            commandText: sql,
            parameters: entity,
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);
    }

    protected CommandDefinition DeleteBaseCommand(Guid id, CancellationToken ct)
    {
        var sql = $"DELETE FROM {SCHEMA_NAME}.{TableName} WHERE id = @id";

        return new CommandDefinition(
            commandText: sql,
            parameters: new { id },
            transaction: Transaction,
            commandTimeout: SQL_TIMEOUT,
            cancellationToken: ct);
    }
    
    protected string InsertUnderscoreBeforeUpperCase(string str)
    {
        var sb = new StringBuilder();

        char previousChar = char.MinValue;

        foreach (char ch in str)
        {
            bool isCorrectChar = char.IsUpper(ch) && sb.Length != 0 && previousChar != ' ';

            if (isCorrectChar)
            {
                sb.Append('_');
            }

            sb.Append(char.ToLower(ch));

            previousChar = ch;
        }

        return sb.ToString();
    }

    public virtual void Dispose()
    {
        Connection?.Close();
        Connection?.Dispose();
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
