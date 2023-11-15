using System;

namespace ProjectAvalonia.Common.Models;

[AttributeUsage(validOn: AttributeTargets.Field)]
public class FriendlyNameAttribute : Attribute
{
    public FriendlyNameAttribute(
        string friendlyName
    )
    {
        FriendlyName = friendlyName;
    }

    public string FriendlyName
    {
        get;
    }
}