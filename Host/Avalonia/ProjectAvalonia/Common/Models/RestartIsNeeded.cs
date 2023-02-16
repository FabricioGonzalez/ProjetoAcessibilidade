using System;

namespace ProjectAvalonia.Common.Models;
public class RestartNeededEventArgs : EventArgs
{
    public bool IsRestartNeeded
    {
        get; init;
    }
}