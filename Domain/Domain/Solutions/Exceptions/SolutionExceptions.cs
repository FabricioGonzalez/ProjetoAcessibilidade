namespace Domain.Solutions.Exceptions;

public static class SolutionExceptions
{
    public static Exception NoSolutionFoundException => new InvalidOperationException("Solution cannot be found");
}