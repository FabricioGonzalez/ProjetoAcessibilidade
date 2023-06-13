using System.Windows.Input;

using Avalonia.Input;
using Avalonia.Media;

namespace ProjectAvalonia.Models;
public interface IMenuItem
{
    string Label
    {
        get;
    }

    StreamGeometry Icon
    {
        get;
    }

    ICommand Command
    {
        get;
    }

    KeyGesture Gesture
    {
        get;
    }
}
