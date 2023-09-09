using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProjectAvalonia.Common.Microservices;

/// <summary>
///     Factory for <see cref="ProcessStartInfo" /> with pre-defined properties as needed in Wasabi Wallet.
/// </summary>
public class ProcessStartInfoFactory
{
    /// <summary>
    ///     Creates new <see cref="ProcessStartInfo" /> instance.
    /// </summary>
    /// <param name="processPath">Path to process.</param>
    /// <param name="arguments">Process arguments.</param>
    /// <param name="openConsole">Open console window. Only for Windows platform.</param>
    /// <returns><see cref="ProcessStartInfo" /> instance.</returns>
    public static ProcessStartInfo Make(
        string processPath
        , string arguments
        , bool openConsole = false
    )
    {
        ProcessWindowStyle windowStyle;

        if (openConsole)
        {
            if (!RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException(
                    message: $"{RuntimeInformation.OSDescription} is not supported.");
            }

            windowStyle = ProcessWindowStyle.Normal;
        }
        else
        {
            windowStyle = ProcessWindowStyle.Hidden;
        }

        var p = new ProcessStartInfo(fileName: processPath, arguments: arguments)
        {
            FileName = processPath, Arguments = arguments, RedirectStandardOutput = !openConsole
            , UseShellExecute = openConsole, CreateNoWindow = !openConsole, WindowStyle = windowStyle
        };

        return p;
    }
}