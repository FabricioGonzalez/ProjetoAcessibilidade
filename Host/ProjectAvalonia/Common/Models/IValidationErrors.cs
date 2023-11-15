namespace ProjectAvalonia.Common.Models;

public interface IValidationErrors
{
    void Add(
        ErrorSeverity severity
        , string error
    );
}