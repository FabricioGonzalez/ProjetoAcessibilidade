using System;
using System.IO;
using System.Runtime.InteropServices;
using ProjectAvalonia.Common.Helpers;

namespace ProjectAvalonia.Common.Microservices;

public static class MicroserviceHelpers
{
    public static OSPlatform GetCurrentPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Windows))
        {
            return OSPlatform.Windows;
        }

        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.Linux))
        {
            return OSPlatform.Linux;
        }

        if (RuntimeInformation.IsOSPlatform(osPlatform: OSPlatform.OSX))
        {
            return OSPlatform.OSX;
        }

        throw new NotSupportedException(message: "Platform is not supported.");
    }

    public static string GetBinaryFolder(
        OSPlatform? platform = null
    )
    {
        platform ??= GetCurrentPlatform();

        var fullBaseDirectory = EnvironmentHelpers.GetFullBaseDirectory();
        var commonPartialPath = Path.Combine(path1: fullBaseDirectory, path2: "Microservices", path3: "Binaries");

        string path;
        if (platform == OSPlatform.Windows)
        {
            path = Path.Combine(path1: commonPartialPath, path2: "win64");
        }
        else if (platform == OSPlatform.Linux)
        {
            path = Path.Combine(path1: commonPartialPath, path2: "lin64");
        }
        else if (platform == OSPlatform.OSX)
        {
            path = Path.Combine(path1: commonPartialPath, path2: "osx64");
        }
        else
        {
            throw new NotSupportedException(message: "Operating system is not supported.");
        }

        return path;
    }

    public static string GetBinaryPath(
        string binaryNameWithoutExtension
        , OSPlatform? platform = null
    )
    {
        platform ??= GetCurrentPlatform();
        var binaryFolder = GetBinaryFolder(platform: platform);
        var fileName = platform.Value == OSPlatform.Windows
            ? $"{binaryNameWithoutExtension}.exe"
            : $"{binaryNameWithoutExtension}";

        return Path.Combine(path1: binaryFolder, path2: fileName);
    }
}