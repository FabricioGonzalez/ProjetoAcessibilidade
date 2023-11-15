using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectAvalonia.Common.Http.Extensions;

public static class HttpContentExtensions
{
    public static async Task<T> ReadAsJsonAsync<T>(
        this HttpContent me
    )
    {
        if (me is null)
        {
            throw new ArgumentNullException(paramName: nameof(me));
        }

        var settings = new JsonSerializerSettings();
        var jsonString = await me.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        return JsonConvert.DeserializeObject<T>(value: jsonString, settings: settings)
               ?? throw new InvalidOperationException(message: $"Deserialization failed. Received json: {jsonString}");
    }
}