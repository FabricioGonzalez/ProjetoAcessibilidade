namespace Domain.Rules.Exceptions;

public static class RulesExceptions
{
    public static Exception NoSolutionFoundException => new InvalidOperationException("Solution cannot be found");
}