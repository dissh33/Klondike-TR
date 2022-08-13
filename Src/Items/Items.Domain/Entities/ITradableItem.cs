namespace Items.Domain.Entities;

public interface ITradableItem
{
    public Guid Id { get; }
    public Icon? Icon { get; }  
    public Guid IconId { get; }
}