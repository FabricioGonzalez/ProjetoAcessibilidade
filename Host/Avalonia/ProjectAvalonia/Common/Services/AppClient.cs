using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Http;
using ProjectAvalonia.Common.Http.Extensions;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Common.Services;

public class AppClient
{
    public AppClient(IHttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    private IHttpClient HttpClient
    {
        get;
    }
    public async Task<(Version ClientVersion, ushort BackendMajorVersion, Version LegalDocumentsVersion)> GetVersionsAsync(CancellationToken cancel)
    {
        using HttpResponseMessage response =
        await HttpClient
        .SendAsync(HttpMethod.Get, "/api/software/versions", cancellationToken: cancel)
        .ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            /*   await response.ThrowRequestExceptionFromContentAsync(cancel).ConfigureAwait(false); */
        }

        using HttpContent content = response.Content;
        var resp = await content.ReadAsJsonAsync<string>().ConfigureAwait(false);

        return (Version.Parse("1"), ushort.Parse("1"), Version.Parse("1"));
    }
    public async Task<UpdateStatus> CheckUpdatesAsync(CancellationToken cancel)
    {
        var (clientVersion, backendMajorVersion, legalDocumentsVersion) = await GetVersionsAsync(cancel).ConfigureAwait(false);
        var clientUpToDate = AppConstants.ClientVersion >= clientVersion; // If the client version locally is greater than or equal to the backend's reported client version, then good.
        var backendCompatible = int.Parse(AppConstants.ClientSupportBackendVersionMax)
        >= backendMajorVersion && backendMajorVersion
        >= int.Parse(AppConstants.ClientSupportBackendVersionMin);
        // If ClientSupportBackendVersionMin <= backend major <= ClientSupportBackendVersionMax, then our software is compatible.
        var currentBackendMajorVersion = backendMajorVersion;

        if (backendCompatible)
        {
            // Only refresh if compatible.
            ApiVersion = currentBackendMajorVersion;
        }

        return new UpdateStatus(backendCompatible, clientUpToDate, legalDocumentsVersion, currentBackendMajorVersion, clientVersion);
    }

    public static ushort ApiVersion { get; private set; } = ushort.Parse(AppConstants.BackendMajorVersion);
}
