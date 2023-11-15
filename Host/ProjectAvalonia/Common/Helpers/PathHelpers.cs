using System;

namespace ProjectAvalonia.Common.Helpers;

public static class PathHelpers
{
    public static string UriToFilePath(
        this Uri uri
    ) => uri.AbsolutePath.Replace(oldValue: "%20", newValue: " ");
}