using Items.Domain.Events;

namespace Items.Domain;

public interface IDomainEvents
{
    public List<DomainEvent> DomainEvents { get; set; }
}