using System;
using System.Linq;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Common.Extensions;

public static class EnumExtensions
{
    public static T? GetFirstAttribute<T>(
        this Enum value
    )
        where T : Attribute
    {
        var stringValue = value.ToString();
        var type = value.GetType();
        var memberInfo =
            type.GetMember(name: stringValue)
                .FirstOrDefault()
            ?? throw new InvalidOperationException(
                message: $"Enum of type '{typeof(T).FullName}' does not contain value '{stringValue}'");

        var attributes = memberInfo.GetCustomAttributes(attributeType: typeof(T), inherit: false);

        return attributes.Any() ? (T)attributes[0] : null;
    }

    public static string FriendlyName(
        this Enum value
    )
    {
        var attribute = value.GetFirstAttribute<FriendlyNameAttribute>();

        return attribute is not null ? attribute.FriendlyName : value.ToString();
    }

    public static T? GetEnumValueOrDefault<T>(
        this int value
        , T defaultValue
    )
        where T : Enum
    {
        if (Enum.IsDefined(enumType: typeof(T), value: value))
        {
            return (T?)Enum.ToObject(enumType: typeof(T), value: value);
        }

        return defaultValue;
    }
}