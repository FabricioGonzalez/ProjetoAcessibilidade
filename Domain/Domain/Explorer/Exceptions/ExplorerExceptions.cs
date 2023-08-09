namespace Domain.Explorer.Exceptions;

public static class ExplorerExceptions
{
    public static Exception NoItemsWasFoundException => new InvalidOperationException("No Items Was Found");
}