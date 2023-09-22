using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Http;
using ProjectAvalonia.Common.Http.Extensions;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Common.Services;

public class AppClient
{
    public AppClient(
        IHttpClient httpClient
    )
    {
        HttpClient = httpClient;
    }

    private IHttpClient HttpClient
    {
        get;
    }

    public static ushort ApiVersion
    {
        get;
        private set;
    } = ushort.Parse(AppConstants.BackendMajorVersion);

    public async Task<Version> GetVersionFromGithub()
    {
        using var message = new HttpRequestMessage(HttpMethod.Get, AppConstants.ReleaseURL);
        message.Headers.UserAgent.Add(new ProductInfoHeaderValue("ProjetoAcessibilidade"
            , "2.0"));
        var response = await HttpClient.SendAsync(message, CancellationToken.None)
            .ConfigureAwait(false);
        var jsonResponse = JObject.Parse(await response.Content
            .ReadAsStringAsync(CancellationToken.None)
            .ConfigureAwait(false));

        var softwareVersion = jsonResponse["tag_name"]?.ToString()
                              ?? throw new InvalidDataException(
                                  "Endpoint gave back wrong json data or it's changed.");

        // "tag_name" will have a 'v' at the beggining, needs to be removed.
        Version githubVersion = new(softwareVersion[1..]);
        Version shortGithubVersion =
            new(githubVersion.Major, githubVersion.Minor, githubVersion.Build);

        return shortGithubVersion;
    }

    public async Task<(Version ClientVersion, ushort BackendMajorVersion, Version LegalDocumentsVersion)>
        GetVersionsAsync(
            CancellationToken cancel
        )
    {
        try
        {
            using var response =
                await HttpClient
                    .SendAsync(HttpMethod.Get, "", cancellationToken: cancel)
                    .ConfigureAwait(false);

            using var content = response.EnsureSuccessStatusCode().Content;
            var resp = await content.ReadAsJsonAsync<string>().ConfigureAwait(false);

            return (Version.Parse("1"), ushort.Parse("1"), Version.Parse("1"));
        }

        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return (Version.Parse("1.0.0"), ushort.Parse("1"), Version.Parse("1.0.0"));
        }
    }

    public async Task<UpdateStatus> CheckUpdatesAsync(
        CancellationToken cancel
    )
    {
        /* var (clientVersion, backendMajorVersion, legalDocumentsVersion) =
             await GetVersionsAsync(cancel).ConfigureAwait(false);*/

        var appVersion = await GetVersionFromGithub().ConfigureAwait(false);

        /*var currentVersion = Version.Parse(ServicesConfig.Config.AppVersion);*/

        var ver = EnvironmentHelpers.GetExecutableVersion();

        var clientUpToDate = /*AppConstants.ClientVersion >= clientVersion ||*/
            Version.Parse(ver) >=
            appVersion; // If the client version locally is greater than or equal to the backend's reported client version, then good.
        var backendCompatible = int.Parse(AppConstants.ClientSupportBackendVersionMax)
                                >= int.Parse(AppConstants.ClientSupportBackendVersionMin);
        // If ClientSupportBackendVersionMin <= backend major <= ClientSupportBackendVersionMax, then our software is compatible.

        return new UpdateStatus(
            clientUpToDate,
            appVersion);
    }
}