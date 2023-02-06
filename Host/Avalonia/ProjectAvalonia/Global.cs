using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

using Nito.AsyncEx;

using ProjectAvalonia.Common.Http;
using ProjectAvalonia.Common.Services;
using ProjectAvalonia.Common.Services.Terminate;
using ProjectAvalonia.Logging;

namespace ProjectAvalonia;

public class Global
{
    public const string ThemeBackgroundBrushResourceKey = "ThemeBackgroundBrush";
    public const string ApplicationAccentForegroundBrushResourceKey = "ApplicationAccentForegroundBrush";

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
        get; set;
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
        get; private set;
    }

    public Global(string dataDir, Config config, UiConfig uiConfig)
    {
        DataDir = dataDir;
        Config = config;
        UiConfig = uiConfig;

        HostedServices = new HostedServices();

        UpdateManager = new(DataDir, Config.DownloadNewVersion, new ProjectHttpClient(new System.Net.Http.HttpClient()));

        Cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 1_000,
            ExpirationScanFrequency = TimeSpan.FromSeconds(30)
        });
    }

    /// <remarks>Use this variable as a guard to prevent touching <see cref="StoppingCts"/> that might have already been disposed.</remarks>
    private volatile bool _disposeRequested;

    /// <summary>Lock that makes sure the application initialization and dispose methods do not run concurrently.</summary>
    private AsyncLock InitializationAsyncLock { get; } = new();

    /// <summary>Cancellation token to cancel <see cref="InitializeNoWalletAsync(TerminateService)"/> processing.</summary>
    private CancellationTokenSource StoppingCts { get; } = new();

    public async Task InitializeNoWalletAsync(TerminateService terminateService)
    {
        // StoppingCts may be disposed at this point, so do not forward the cancellation token here.
        using (await InitializationAsyncLock.LockAsync())
        {
            Logger.LogTrace("Initialization started.");

            if (_disposeRequested)
            {
                return;
            }

            CancellationToken cancel = StoppingCts.Token;

            try
            {
                /*  HostedServices.Register<UpdateChecker>(() => new UpdateChecker(TimeSpan.FromMinutes(7)), "Software Update Checker");
                  var updateChecker = HostedServices.Get<UpdateChecker>();

                  UpdateManager.Initialize(updateChecker, cancel);*/

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
            Logger.LogWarning("Process is exiting.", nameof(Global));

            try
            {
                if (UpdateManager is { } updateManager)
                {
                    UpdateManager.Dispose();
                    Logger.LogInfo($"{nameof(UpdateManager)} is stopped.", nameof(Global));
                }

                if (HostedServices is { } backgroundServices)
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(21));
                    await backgroundServices.StopAllAsync(cts.Token).ConfigureAwait(false);
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
