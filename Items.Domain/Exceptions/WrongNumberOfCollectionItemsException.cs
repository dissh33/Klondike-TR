namespace Items.Domain.Exceptions;

public class WrongNumberOfCollectionItemsException : Exception
{
    public WrongNumberOfCollectionItemsException(int number)
    : base($"Collection must have exactly 5 items (specified number is {number}).")
    {
        
    }
}