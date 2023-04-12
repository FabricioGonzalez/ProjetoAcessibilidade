using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectWinUI.Src.Helpers;

public static class Json
{
    public static async Task<T> ToObjectAsync<T>(
        string value
    ) =>
        await Task.Run(function: () =>
        {
            return JsonSerializer.Deserialize<T>(json: value);
        });

    public static async Task<string> StringifyAsync(
        object value
    ) =>
        await Task.Run(function: () =>
        {
            return JsonSerializer.Serialize(value: value);
        });
}