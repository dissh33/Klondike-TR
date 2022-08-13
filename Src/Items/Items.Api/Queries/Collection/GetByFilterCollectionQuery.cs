using Items.Api.Dtos;
using MediatR;

namespace Items.Api.Queries.Collection;

public class GetByFilterCollectionQuery : IRequest<IEnumerable<CollectionDto>>
{
    public string? Name { get; set; }
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string GenerateSql()
    {
        var conditions = new List<string>();

        if (!string.IsNullOrWhiteSpace(Name))
        {
            conditions.Add($"  name ~* '{Name}'");
        }

        if (Status != null)
        {
            conditions.Add($" status = '{Status}'");    
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