using System.Data;
using System.Text;
using App.Metrics;
using Dapper;
using Items.Application.Contracts;
using Items.Domain.Entities;
using Newtonsoft.Json;
using Serilog;

namespace Items.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected const int SqlTimeout = 3600;
    protected const string SchemaName = "public";

    protected IDbTransaction Transaction { get; }
    protected ILogger Logger { get; }
    protected IMetrics Metrics { get; }
    
    protected IDbConnection? Connection { get; }

    protected string TableName { get; }

    protected static List<string> ExcludeProperties = new() { "DomainEvents" };

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
            .Where(prop => !ExcludeProperties.Contains(prop.Name))
            .Select(prop => prop.Name);
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
