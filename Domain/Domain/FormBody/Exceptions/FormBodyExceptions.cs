namespace Domain.FormBody.Exceptions;

public static class FormBodyExceptions
{
    public static Exception ItemIsNotAFileException => new InvalidOperationException("This item is not a File");
}