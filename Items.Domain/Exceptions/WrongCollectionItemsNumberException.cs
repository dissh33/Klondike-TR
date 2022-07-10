namespace Items.Domain.Exceptions;

public class WrongCollectionItemsNumberException : Exception
{
    public WrongCollectionItemsNumberException(int number)
    : base($"Collection must have exactly 5 items (specified number is {number}).")
    {
        
    }
}