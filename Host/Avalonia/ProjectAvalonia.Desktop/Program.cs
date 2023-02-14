using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.OpenGL;
using Avalonia.ReactiveUI;

using Common;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Services.Terminate;
using ProjectAvalonia.Desktop.Extensions;
using ProjectAvalonia.Logging;
using ProjectAvalonia.ViewModels;

using ReactiveUI;

using Splat;

using LogLevel = ProjectAvalonia.Logging.LogLevel;

namespace ProjectAvalonia.Desktop;

public class Program
{
    private static Global? Global;

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    public static int Main(string[] args)
    {
        var runGuiInBackground = args.Any(arg => arg.Contains(StartupHelper.SilentArgument));

        // Initialize the logger.
        var dataDir = EnvironmentHelpers.GetDataDir(Path.Combine(Constants.AppName));
        SetupLogger(dataDir, args);

        Logger.LogDebug($"{Constants.AppName} was started with these argument(s): {(args.Any() ? string.Join(" ", args) : "none")}.");

        // Crash reporting must be before the "single instance checking".
        try
        {
            /*  if (CrashReporter.TryGetExceptionFromCliArgs(args, out var exceptionToShow))
              {
                  // Show the exception.
                  BuildCrashReporterApp(exceptionToShow).StartWithClassicDesktopLifetime(args);
                  return 1;
              }*/
        }
        catch (Exception ex)
        {
            // If anything happens here just log it and exit.
            Logger.LogCritical(ex);
            return 1;
        }

        (UiConfig uiConfig, Config config) = LoadOrCreateConfigs(dataDir);

        // Start single instance checker that is active over the lifetime of the application.
        /*        using SingleInstanceChecker singleInstanceChecker = new(config.Network);

                try
                {
                    singleInstanceChecker.EnsureSingleOrThrowAsync().GetAwaiter().GetResult();
                }
                catch (OperationCanceledException)
                {
                    // We have successfully signalled the other instance and that instance should pop up
                    // so user will think he has just run the application.
                    return 1;
                }
                catch (Exception ex)
                {
                    CrashReporter.Invoke(ex);
                    Logger.LogCritical(ex);
                    return 1;
                }*/

        // Now run the GUI application.
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        Exception? exceptionToReport = null;
        TerminateService terminateService = new(TerminateApplicationAsync, TerminateApplication);

        try
        {
            Global = CreateGlobal(dataDir, uiConfig, config);
            ServicesConfig.Initialize(Global);

            RxApp.DefaultExceptionHandler = Observer.Create<Exception>(ex =>
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    Logger.LogError(ex);

                    RxApp.MainThreadScheduler.Schedule(() => throw new ApplicationException("Exception has been thrown in unobserved ThrownExceptions", ex));
                });

            Logger.LogSoftwareStarted("Gestor de Projeto ARPA GUI");
            AppBuilder
                .Configure(
                () =>
                new App(
                    async () =>
                    await Global.InitializeNoWalletAsync(terminateService), runGuiInBackground))
                .UseReactiveUI()
                .AddQueryHandlers()
                .AddCommandHandlers()
                .AddRepositories()
                .AddMediator()
                .SetupAppBuilder()
                .AfterSetup(_ =>
                    {
                        var glInterface = AvaloniaLocator.CurrentMutable.GetService<IPlatformOpenGlInterface>();
                        Logger.LogInfo(glInterface is { }
                            ? $"Renderer: {glInterface.PrimaryContext.GlInterface.Renderer}"
                            : "Renderer: Avalonia Software");

                        ThemeHelper.ApplyTheme(Global.UiConfig.DarkModeEnabled ? Theme.Dark : Theme.Light);
                    })
                    .StartWithClassicDesktopLifetime(args);
        }
        catch (OperationCanceledException ex)
        {
            Logger.LogDebug(ex);
        }
        catch (Exception ex)
        {
            exceptionToReport = ex;
            Logger.LogCritical(ex);
        }

        // Start termination/disposal of the application.
        terminateService.Terminate();

        if (exceptionToReport is { })
        {
            // Trigger the CrashReport process if required.
            /* CrashReporter.Invoke(exceptionToReport);*/
        }
        /* else if (ServicesConfig.UpdateManager.DoUpdateOnClose)
         {
             ServicesConfig.UpdateManager.StartInstallingNewVersion();
         }*/

        AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;

        Logger.LogSoftwareStopped(Constants.AppName);

        return exceptionToReport is { } ? 1 : 0;
    }

    /// <summary>
    /// Initializes Wasabi Logger. Sets user-defined log-level, if provided.
    /// </summary>
    /// <example>Start Wasabi Wallet with <c>./wassabee --LogLevel=trace</c> to set <see cref="LogLevel.Trace"/>.</example>
    private static void SetupLogger(string dataDir, string[] args)
    {
        LogLevel? logLevel = null;

        foreach (var arg in args)
        {
            if (arg.StartsWith("--LogLevel="))
            {
                var value = arg.Split('=', count: 2)[1];

                if (Enum.TryParse(value, ignoreCase: true, out LogLevel parsedLevel))
                {
                    logLevel = parsedLevel;
                    break;
                }
            }
        }

        Logger.InitializeDefaults(Path.Combine(Constants.AppLogsSettings, "Logs.txt"), logLevel);
    }

    private static (UiConfig uiConfig, Config config) LoadOrCreateConfigs(string dataDir)
    {
        Directory.CreateDirectory(dataDir);
        Directory.CreateDirectory(Constants.AppUISettings);

        UiConfig uiConfig = new(Path.Combine(Constants.AppUISettings, Constants.AppUISettingsFile));
        uiConfig.LoadOrCreateDefaultFile();

        Config config = new(Path.Combine(Constants.AppSettingsFolder, Constants.AppSettingsFile));
        config.LoadOrCreateDefaultFile();

        return (uiConfig, config);
    }

    private static Global CreateGlobal(string dataDir, UiConfig uiConfig, Config config)
    {


        return new Global(dataDir, config, uiConfig);
    }

    /// <summary>
    /// Do not call this method it should only be called by TerminateService.
    /// </summary>
    private static async Task TerminateApplicationAsync()
    {
        Logger.LogSoftwareStopped("Gestor de Projeto ARPA GUI");

        if (Global is { } global)
        {
            await global.DisposeAsync().ConfigureAwait(false);
        }
    }

    private static void TerminateApplication()
    {
        MainViewModel.Instance.ClearStacks();
        /*MainViewModel.Instance.StatusIcon.Dispose();*/
    }

    private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        ReadOnlyCollection<Exception> innerExceptions = e.Exception.Flatten().InnerExceptions;

        if (innerExceptions.Count == 1 && innerExceptions[0] is SocketException socketException && socketException.SocketErrorCode == SocketError.OperationAborted)
        {
            // Until https://github.com/MetacoSA/NBitcoin/pull/1089 is resolved.
            Logger.LogTrace(e.Exception);
        }
        else if (innerExceptions.Count == 1 && innerExceptions[0] is OperationCanceledException ex && ex.Message == "The peer has been disconnected")
        {
            // Source of this exception is NBitcoin library.
            Logger.LogTrace(e.Exception);
        }
        else
        {
            Logger.LogDebug(e.Exception);
        }
    }

    private static void CurrentDomain_UnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            Logger.LogWarning(ex);
        }
    }

    [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Required to bootstrap Avalonia's Visual Previewer")]
    private static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure(() => new App()).UseReactiveUI().SetupAppBuilder();

    /*    /// <summary>
        /// Sets up and initializes the crash reporting UI.
        /// </summary>
        /// <param name="serializableException"></param>
        /// <param name="logPath"></param>
        /// <returns></returns>
        private static AppBuilder BuildCrashReporterApp(SerializableException serializableException)
        {
            *//* var result = AppBuilder
                 .Configure(() => new CrashReportApp(serializableException))
                 .UseReactiveUI();

             if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
             {
                 result
                     .UseWin32()
                     .UseSkia();
             }
             else
             {
                 result.UsePlatformDetect();
             }

             return result
                 .With(new Win32PlatformOptions { AllowEglInitialization = false, UseDeferredRendering = true })
                 .With(new X11PlatformOptions { UseGpu = false, WmClass = "Wasabi Wallet Crash Reporting" })
                 .With(new AvaloniaNativePlatformOptions { UseDeferredRendering = true, UseGpu = false })
                 .With(new MacOSPlatformOptions { ShowInDock = true })
                 .AfterSetup(_ => ThemeHelper.ApplyTheme(Theme.Dark));*//*
        }*/
}
