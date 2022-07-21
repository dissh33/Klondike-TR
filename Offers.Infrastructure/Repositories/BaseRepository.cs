﻿using App.Metrics;
using Dapper;
using Serilog;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Offers.Application.Contracts;
using Offers.Domain.SeedWork;

namespace Offers.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseGenericRepository<T> where T : BaseEntity
{
    protected const int SQL_TIMEOUT = 3600;
    protected const string SCHEMA_NAME = "public";

    protected IDbTransaction Transaction { get; }
    protected ILogger Logger { get; }
    protected IMetrics Metrics { get; }

    protected IDbConnection? Connection { get; }

    protected string TableName { get; }

    protected List<string> ExcludeProperties = new() {  "DomainEvents" };

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