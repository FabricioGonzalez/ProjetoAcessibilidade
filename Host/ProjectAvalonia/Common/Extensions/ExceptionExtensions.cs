using System;
using ProjectAvalonia.Common.Exceptions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Common.Extensions;

public static class ExceptionExtensions
{
    public static string ToTypeMessageString(
        this Exception ex
    )
    {
        var trimmed = Guard.Correct(str: ex.Message);

        if (trimmed.Length == 0)
        {
            if (ex is HwiException hwiEx)
            {
                return $"{hwiEx.GetType().Name}: {hwiEx.ErrorCode}";
            }

            return ex.GetType().Name;
        }

        return $"{ex.GetType().Name}: {ex.Message}";
    }

    public static string ToUserFriendlyString(
        this Exception ex
    )
    {
        var trimmed = Guard.Correct(str: ex.Message);
        if (trimmed.Length == 0)
        {
            return ex.ToTypeMessageString();
        }

        if (ex is OperationCanceledException exception)
        {
            return exception.Message;
        }

        if (ex is HwiException hwiEx)
        {
            if (hwiEx.ErrorCode == HwiErrorCode.DeviceConnError)
            {
                return "Could not find the hardware wallet.\nMake sure it is connected.";
            }

            if (hwiEx.ErrorCode == HwiErrorCode.ActionCanceled)
            {
                return "The transaction was canceled on the device.";
            }

            if (hwiEx.ErrorCode == HwiErrorCode.UnknownError)
            {
                return "Unknown error.\nMake sure the device is connected and isn't busy, then try again.";
            }
        }

        /* foreach (KeyValuePair<string, string> pair in RpcErrorTools.ErrorTranslations)
             {
                 if (trimmed.Contains(pair.Key, StringComparison.InvariantCultureIgnoreCase))
                 {
                     return pair.Value;
                 }
             }*/

        return ex.ToTypeMessageString();
    }

    public static SerializableException ToSerializableException(
        this Exception ex
    ) => new(ex: ex);
}