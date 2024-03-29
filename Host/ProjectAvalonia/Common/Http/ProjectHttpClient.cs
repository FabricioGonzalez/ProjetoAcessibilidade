using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectAvalonia.Common.Http;

/// <summary>
///     HTTP client implementation based on .NET's <see cref="HttpClient" /> which provides least privacy for Wasabi users,
///     as HTTP requests are being sent over clearnet.
/// </summary>
/// <remarks>Inner <see cref="HttpClient" /> instance is thread-safe.</remarks>
public class ProjectHttpClient : IHttpClient
{
    public ProjectHttpClient(
        HttpClient httpClient
    )
    {
        BaseUriGetter = () => httpClient.BaseAddress ??
                              throw new NotSupportedException(message: "No base address was set.");
        HttpClient = httpClient;
    }

    public ProjectHttpClient(
        HttpClient httpClient
        , Func<Uri>? baseUriGetter
    )
    {
        BaseUriGetter = baseUriGetter;
        HttpClient = httpClient;
    }

    /// <summary>Predefined HTTP client that handles HTTP requests when Tor is disabled.</summary>
    private HttpClient HttpClient
    {
        get;
    }

    public Func<Uri>? BaseUriGetter
    {
        get;
    }

    /// <inheritdoc cref="HttpClient.SendAsync(HttpRequestMessage, CancellationToken)" />
    public virtual Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request
        , CancellationToken token = default
    ) => HttpClient.SendAsync(request: request, cancellationToken: token);
}