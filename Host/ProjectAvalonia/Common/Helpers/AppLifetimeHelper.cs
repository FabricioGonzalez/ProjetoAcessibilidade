using System;
using System.Diagnostics;
using Avalonia.Controls.ApplicationLifetimes;
using ProjectAvalonia.Common.Microservices;
using ProjectAvalonia.ViewModels;

namespace ProjectAvalonia.Common.Helpers;

/// <summary>
///     Helper methods for Domain.Application lifetime related functions.
/// </summary>
public static class AppLifetimeHelper
{
    /// <summary>
    ///     Attempts to start a new instance of the app with optional program arguments
    /// </summary>
    /// <remarks>
    ///     This method is only functional on the published builds
    ///     and not on debugging runs.
    /// </remarks>
    /// <param name="args">The program arguments to pass to the new instance.</param>
    public static void StartAppWithArgs(
        string args = ""
    )
    {
        var path = Process.GetCurrentProcess().MainModule?.FileName;

        if (string.IsNullOrEmpty(value: path))
        {
            throw new InvalidOperationException(message: $"Invalid path: '{path}'");
        }

        var startInfo = ProcessStartInfoFactory.Make(processPath: path, arguments: args);
        using var p = Process.Start(startInfo: startInfo);
    }

    /// <summary>
    ///     Shuts down the application safely, optionally shutdown prevention and restart can be requested.
    /// </summary>
    /// <remarks>
    ///     This method is only functional on the published builds
    ///     and not on debugging runs.
    /// </remarks>
    /// <param name="withShutdownPrevention">
    ///     Enabled the shutdown prevention, so a dialog will pop until the shutdown can be
    ///     done safely.
    /// </param>
    /// <param name="restart">If true, the application will restart after shutdown.</param>
    public static void Shutdown(
        bool withShutdownPrevention = true
        , bool restart = false
    )
    {
        switch ((withShutdownPrevention, restart))
        {
            case (true, true):
            case (true, false):
                (Avalonia.Application.Current?.DataContext as ApplicationViewModel)?.Shutdown(restart: restart);
                break;

            case (false, true):
                StartAppWithArgs();
                (Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
                    ?.Shutdown();
                break;

            case (false, false):
                (Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
                    ?.Shutdown();
                break;
        }
    }
}