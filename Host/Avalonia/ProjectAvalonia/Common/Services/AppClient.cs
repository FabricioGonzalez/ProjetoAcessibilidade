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
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Logging;

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
    } = ushort.Parse(s: AppConstants.BackendMajorVersion);

    public async Task<Version> GetVersionFromGithub()
    {
        using var message = new HttpRequestMessage(method: HttpMethod.Get, requestUri: AppConstants.ReleaseURL);
        message.Headers.UserAgent.Add(item: new ProductInfoHeaderValue(productName: "ProjetoAcessibilidade"
            , productVersion: "2.0"));
        var response = await HttpClient.SendAsync(request: message, cancellationToken: CancellationToken.None)
            .ConfigureAwait(continueOnCapturedContext: false);
        var jsonResponse = JObject.Parse(json: await response.Content
            .ReadAsStringAsync(cancellationToken: CancellationToken.None)
            .ConfigureAwait(continueOnCapturedContext: false));

        var softwareVersion = jsonResponse[propertyName: "tag_name"]?.ToString()
                              ?? throw new InvalidDataException(
                                  message: "Endpoint gave back wrong json data or it's changed.");

        // "tag_name" will have a 'v' at the beggining, needs to be removed.
        Version githubVersion = new(version: softwareVersion[1..]);
        Version shortGithubVersion =
            new(major: githubVersion.Major, minor: githubVersion.Minor, build: githubVersion.Build);

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
                    .SendAsync(method: HttpMethod.Get, relativeUri: "", cancellationToken: cancel)
                    .ConfigureAwait(continueOnCapturedContext: false);

            using var content = response.EnsureSuccessStatusCode().Content;
            var resp = await content.ReadAsJsonAsync<string>().ConfigureAwait(continueOnCapturedContext: false);

            return (Version.Parse(input: "1"), ushort.Parse(s: "1"), Version.Parse(input: "1"));
        }

        catch (Exception ex)
        {
            Logger.LogError(message: ex.ToString());
            return (Version.Parse(input: "1.0.0"), ushort.Parse(s: "1"), Version.Parse(input: "1.0.0"));
        }
    }

    public async Task<UpdateStatus> CheckUpdatesAsync(
        CancellationToken cancel
    )
    {
        /* var (clientVersion, backendMajorVersion, legalDocumentsVersion) =
             await GetVersionsAsync(cancel).ConfigureAwait(false);*/

        var appVersion = await GetVersionFromGithub().ConfigureAwait(continueOnCapturedContext: false);

        /*var currentVersion = Version.Parse(ServicesConfig.Config.AppVersion);*/

        var ver = EnvironmentHelpers.GetExecutableVersion();

        var clientUpToDate = /*AppConstants.ClientVersion >= clientVersion ||*/
            Version.Parse(input: ver) >=
            appVersion; // If the client version locally is greater than or equal to the backend's reported client version, then good.
        var backendCompatible = int.Parse(s: AppConstants.ClientSupportBackendVersionMax)
                                >= int.Parse(s: AppConstants.ClientSupportBackendVersionMin);
        // If ClientSupportBackendVersionMin <= backend major <= ClientSupportBackendVersionMax, then our software is compatible.

        return new UpdateStatus(
            clientUpToDate: clientUpToDate,
            clientVersion: appVersion);
    }
}