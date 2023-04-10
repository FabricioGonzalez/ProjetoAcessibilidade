using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ProjectAvalonia.Common.Helpers;

public static class AssetHelpers
{
    public static Bitmap GetBitmapAsset(
        Uri uri
    )
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        if (assets is not null)
        {
            using var image = assets.Open(uri: uri);
            return new Bitmap(stream: image);
        }

        throw new Exception(message: "Program is not initialised or is in an inconsistent state.");
    }

    public static Bitmap GetBitmapAsset(
        string path
    ) => GetBitmapAsset(uri: new Uri(uriString: path));
}