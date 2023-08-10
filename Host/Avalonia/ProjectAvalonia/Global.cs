using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Internals.Contracts;
using Microsoft.Extensions.Caching.Memory;
using ProjectAvalonia.Common.Http;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Common.Services;
using ProjectAvalonia.Common.Services.Terminate;
using ProjectAvalonia.Nito.AsyncEx;

namespace ProjectAvalonia;

public class Global
{
    public const string ThemeBackgroundBrushResourceKey = "ThemeBackgroundBrush";
    public const string ApplicationAccentForegroundBrushResourceKey = "ApplicationAccentForegroundBrush";

    /// <remarks>
    ///     Use this variable as a guard to prevent touching <see cref="StoppingCts" /> that might have already been
    ///     disposed.
    /// </remarks>
    private volatile bool _disposeRequested;

    public Global(
        string dataDir
        , Config config
        , UiConfig uiConfig
        , ILanguageManager languageManager
    )
    {
        DataDir = dataDir;
        Config = config;
        UiConfig = uiConfig;

        LanguageManager = languageManager;

        HostedServices = new HostedServices();

        UpdateManager = new UpdateManager(dataDir: DataDir, downloadNewVersion: Config.DownloadNewVersion
            , httpClient: new ProjectHttpClient(new HttpClient()));

        Cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 1_000, ExpirationScanFrequency = TimeSpan.FromSeconds(30)
        });
    }

    public string DataDir
    {
        get;
    }


    /// <summary>HTTP client factory for sending HTTP requests.</summary>
    public IHttpClient HttpClientFactory
    {
        get;
    }

    public Config Config
    {
        get;
    }

    public UpdateManager UpdateManager
    {
        get;
        set;
    }

    public HostedServices HostedServices
    {
        get;
    }

    public UiConfig UiConfig
    {
        get;
    }

    public MemoryCache Cache
    {
        get;
        private set;
    }

    /// <summary>Lock that makes sure the application initialization and dispose methods do not run concurrently.</summary>
    private AsyncLock InitializationAsyncLock
    {
        get;
    } = new();

    /// <summary>Cancellation token to cancel <see cref="InitializeNoWalletAsync(TerminateService)" /> processing.</summary>
    private CancellationTokenSource StoppingCts
    {
        get;
    } = new();

    public ILanguageManager LanguageManager
    {
        get;
        set;
    }

    public async Task InitializeNoWalletAsync(
        TerminateService terminateService
    )
    {
        // StoppingCts may be disposed at this point, so do not forward the cancellation token here.
        using (await InitializationAsyncLock.LockAsync())
        {
            Logger.LogTrace("Initialization started.");

            if (_disposeRequested)
            {
                return;
            }

            var cancel = StoppingCts.Token;

            try
            {
                HostedServices.Register<UpdateChecker>(serviceFactory: () =>
                    new UpdateChecker(TimeSpan.FromMinutes(7))
                    {
                        AppClient = new AppClient(new ProjectHttpClient(httpClient: new HttpClient()
                            , baseUriGetter: () =>
                            {
                                return new Uri(
                                    "https://api.github.com/repos/FabricioGonzalez/ProjetoAcessibilidade/releases");
                            }))
                    }, friendlyName: "Software Update Checker");
                var updateChecker = HostedServices.Get<UpdateChecker>();


                UpdateManager.Initialize(updateChecker: updateChecker, cancelationToken: cancel);

                cancel.ThrowIfCancellationRequested();


                await HostedServices.StartAllAsync(cancel).ConfigureAwait(false);
            }
            finally
            {
                Logger.LogTrace("Initialization finished.");
            }
        }
    }

    public async Task DisposeAsync()
    {
        // Dispose method may be called just once.
        if (!_disposeRequested)
        {
            _disposeRequested = true;
            StoppingCts.Cancel();
        }
        else
        {
            return;
        }

        using (await InitializationAsyncLock.LockAsync())
        {
            Logger.LogWarning(message: "Process is exiting.", callerFilePath: nameof(Global));

            try
            {
                if (UpdateManager is { } updateManager)
                {
                    UpdateManager.Dispose();
                    Logger.LogInfo(message: $"{nameof(UpdateManager)} is stopped.", callerFilePath: nameof(Global));
                }

                if (HostedServices is { } backgroundServices)
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(21));
                    await backgroundServices.StopAllAsync(cts.Token)
                        .ConfigureAwait(false);
                    backgroundServices.Dispose();
                    Logger.LogInfo("Stopped background services.");
                }

                /*RoundStateUpdaterCircuit.Dispose();
                  Logger.LogInfo($"Disposed {nameof(RoundStateUpdaterCircuit)}.");*/

                if (Cache is { } cache)
                {
                    cache.Dispose();
                    Logger.LogInfo($"{nameof(Cache)} is disposed.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex);
            }
            finally
            {
                StoppingCts.Dispose();
                Logger.LogTrace("Dispose finished.");
            }
        }
    }
}