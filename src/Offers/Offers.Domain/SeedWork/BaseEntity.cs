namespace Offers.Domain.SeedWork;

public class BaseEntity
{
    public DateTime CreateDate { get; set; }


    private List<IDomainEvent>? _domainEvents;
    public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();


    public BaseEntity()
    {
        CreateDate = DateTime.Now;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }
    
    internal void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}
