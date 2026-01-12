namespace Offers.Infrastructure.Helpers;

internal class OrderBy(Dictionary<string, string?>? orderByRequest)
{
    public Dictionary<string, string?>? Conditions { get; set; } = orderByRequest;

    public string GenerateSql(bool snakeCase = true)
    {
        if (Conditions == null || Conditions.Count == 0)
        {
            return string.Empty;
        }

        var items = snakeCase
            ? Conditions.ToDictionary(keyValuePair => keyValuePair.Key.ToUnderscoreCase(), keyValuePair => keyValuePair.Value)
            : Conditions;

        var clause = string.Join(",", items
            .Select(keyValuePair => keyValuePair.Key + " " + (keyValuePair.Value?.ToLower() == "desc" ? "DESC" : "ASC")));

        return string.IsNullOrEmpty(clause) ? string.Empty : " ORDER BY " + clause;
    }
}
