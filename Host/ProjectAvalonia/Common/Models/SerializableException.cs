using System;
using System.Text;
using Newtonsoft.Json;

namespace ProjectAvalonia.Common.Models;

[JsonObject(memberSerialization: MemberSerialization.OptIn)]
public record SerializableException
{
    [JsonConstructor]
    protected SerializableException(
        string exceptionType
        , string message
        , string stackTrace
        , SerializableException innerException
    )
    {
        ExceptionType = exceptionType;
        Message = message;
        StackTrace = stackTrace;
        InnerException = innerException;
    }

    public SerializableException(
        Exception ex
    )
    {
        if (ex.InnerException is not null)
        {
            InnerException = new SerializableException(ex: ex.InnerException);
        }

        ExceptionType = ex.GetType().FullName;

        Message = ex.Message;
        StackTrace = ex.StackTrace;
    }

    [JsonProperty(PropertyName = "ExceptionType")]
    public string? ExceptionType
    {
        get;
    }

    [JsonProperty(PropertyName = "Message")]
    public string Message
    {
        get;
    }

    [JsonProperty(PropertyName = "StackTrace")]
    public string? StackTrace
    {
        get;
    }

    [JsonProperty(PropertyName = "InnerException")]
    public SerializableException? InnerException
    {
        get;
    }

    public static string ToBase64String(
        SerializableException exception
    ) => Convert.ToBase64String(inArray: Encoding.UTF8.GetBytes(s: JsonConvert.SerializeObject(value: exception)));

    public static SerializableException FromBase64String(
        string base64String
    )
    {
        var json = Encoding.UTF8.GetString(bytes: Convert.FromBase64String(s: base64String));
        return JsonConvert.DeserializeObject<SerializableException>(value: json);
    }

    public override string ToString() =>
        string.Join(
            separator: Environment.NewLine + Environment.NewLine,
            $"Exception type: {ExceptionType}",
            $"Message: {Message}",
            $"Stack Trace: {StackTrace}",
            $"Inner Exception: {InnerException}");
}