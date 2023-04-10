using System;

namespace ProjectAvalonia.Common.Exceptions;

public class HwiException : Exception
{
    public HwiException(
        HwiErrorCode errorCode
        , string message
    ) : base(message: message)
    {
        ErrorCode = errorCode;
    }

    public HwiErrorCode ErrorCode
    {
        get;
    }
}