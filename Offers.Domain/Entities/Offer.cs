namespace Offers.Domain.Entities;

public class Offer : BaseEntity
{
    public string? Title { get; set; }
    public int Type { get; set; }
    public int Status { get; set; }
    public string? Message { get; set; }
    public string? Expression { get; set; }
}
