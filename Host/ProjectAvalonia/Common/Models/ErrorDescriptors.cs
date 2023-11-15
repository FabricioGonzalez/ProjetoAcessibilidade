using System.Collections.Generic;

namespace ProjectAvalonia.Common.Models;

public class ErrorDescriptors
    : List<ErrorDescriptor>
        , IValidationErrors
{
    public static ErrorDescriptors Empty = Create();

    private ErrorDescriptors()
    {
    }

    void IValidationErrors.Add(
        ErrorSeverity severity
        , string error
    ) => Add(item: new ErrorDescriptor(severity: severity, message: error));

    public static ErrorDescriptors Create() => new();
}