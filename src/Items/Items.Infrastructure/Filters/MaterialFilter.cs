using Items.Api.Queries.Material;

namespace Items.Infrastructure.Filters;

internal class MaterialFilter : MaterialGetByFilterQuery
{
    public string GenerateSql()
    {
        var conditions = new List<string>();

        if (!string.IsNullOrWhiteSpace(Name))
        {
            conditions.Add($"  name ~* '{Name}'");
        }

        if (Type != null)
        {
            conditions.Add($" type = {Type}");
        }

        if (Status != null)
        {
            conditions.Add($" status = {Status}");
        }

        if (StartDate != null)
        {
            conditions.Add($" date >= '{StartDate:yyyy-MM-dd}'");
        }

        if (EndDate != null)
        {
            conditions.Add($" date <= '{EndDate:yyyy-MM-dd}'");
        }

        var whereClause = conditions.Count == 0
            ? string.Empty
            : $" WHERE {string.Join(" AND ", conditions)}";

        return whereClause;
    }
}
