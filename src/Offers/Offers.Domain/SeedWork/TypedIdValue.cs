namespace Offers.Domain.SeedWork;

public abstract class TypedIdValue : IEquatable<TypedIdValue>
{
    public Guid Value { get; }

    protected TypedIdValue()
    {
        Value = Guid.NewGuid();
    }

    protected TypedIdValue(Guid? value)
    {
        Value = value ?? Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is TypedIdValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public bool Equals(TypedIdValue? other)
    {
        return Value == other?.Value;
    }

    public static bool operator ==(TypedIdValue? obj1, TypedIdValue? obj2)
    {
        if (Equals(obj1, null))
        {
            return Equals(obj2, null);
        }
        return obj1.Equals(obj2);
    }

    public static bool operator !=(TypedIdValue? x, TypedIdValue? y)
    {
        return !(x == y);
    }
}