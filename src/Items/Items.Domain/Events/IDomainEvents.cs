namespace Items.Domain.Events;

public interface IDomainEvents
{
    public List<DomainEvent> DomainEvents { get; set; }
}