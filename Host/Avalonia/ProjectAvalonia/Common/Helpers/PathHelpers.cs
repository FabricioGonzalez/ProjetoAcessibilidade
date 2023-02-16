using System;

namespace ProjectAvalonia.Common.Helpers;
public static class PathHelpers
{
    public static string UriToFilePath(this Uri uri)
    {
        return uri.AbsolutePath.Replace("%20", " ");
    }

}
