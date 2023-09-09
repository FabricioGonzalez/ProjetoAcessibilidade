using System.Windows.Input;

using Avalonia.Input;
using Avalonia.Media;

namespace ProjectAvalonia.Models;

public class MenuItemSeparatorModel : MenuItemModel
{
    public MenuItemSeparatorModel()
        : base("-", null, null, null, null)
    {
    }

    private class EmptyMenuItem : IMenuItem
    {
        public string Label => "-";
        public StreamGeometry Icon => null;

        public ICommand Command => null;

        public KeyGesture Gesture => null;
    }
}
