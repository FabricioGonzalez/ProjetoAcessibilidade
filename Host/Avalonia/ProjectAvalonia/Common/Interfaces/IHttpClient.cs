using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectAvalonia.Common.Interfaces;

public interface IHttpClient
{
    Func<Uri>? BaseUriGetter
    {
        get;
    }

    /// <summary>Sends an HTTP(s) request.</summary>
    /// <param name="request">HTTP request message to send.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the asynchronous operation.</param>
    /// <exception cref="HttpRequestException" />
    /// <exception cref="OperationCanceledException">When operation is canceled.</exception>
    Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request
        , CancellationToken cancellationToken = default
    );

    /// <exception cref="HttpRequestException" />
    /// <exception cref="InvalidOperationException" />
    /// <exception cref="OperationCanceledException">When operation is canceled.</exception>
    async Task<HttpResponseMessage> SendAsync(
        HttpMethod method
        , string relativeUri
        , HttpContent? content = null
        , CancellationToken cancellationToken = default
    )
    {
        if (BaseUriGetter is null)
        {
            throw new InvalidOperationException(message: $"{nameof(BaseUriGetter)} is not set.");
        }

        var baseUri = BaseUriGetter.Invoke();

        if (baseUri is null)
        {
            throw new InvalidOperationException(message: "Base URI is not set.");
        }

        Uri requestUri = new(baseUri: baseUri, relativeUri: relativeUri);
        using HttpRequestMessage httpRequestMessage = new(method: method, requestUri: requestUri);

        if (content is not null)
        {
            httpRequestMessage.Content = content;
        }

        return await SendAsync(request: httpRequestMessage, cancellationToken: cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}