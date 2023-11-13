using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ProjectAvalonia.Common.Helpers;

public static class AssetHelpers
{
    public static Bitmap GetBitmapAsset(
        Uri uri
    )
    {
        if (uri is not null)
        {
            return new Bitmap(stream: AssetLoader.Open(uri: uri));
        }

        throw new Exception(message: "Program is not initialised or is in an inconsistent state.");
    }

    public static Bitmap GetBitmapAsset(
        string path
    ) => GetBitmapAsset(uri: new Uri(uriString: path));
}