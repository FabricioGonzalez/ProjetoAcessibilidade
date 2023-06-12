using System;
using System.Windows.Input;

using Avalonia.Media;

namespace ProjectAvalonia.Models;

public class MenuItemSeparatorModel : MenuItemModel
{
    private static readonly Lazy<IMenuItem> EmptyItem = new Lazy<IMenuItem>(() => new EmptyMenuItem());

    public MenuItemSeparatorModel()
        : base(EmptyItem, null)
    {
    }

    private class EmptyMenuItem : IMenuItem
    {
        public string Label => "-";
        public DrawingGroup Icon => null;

        public ICommand Command => null;

        public string Gesture => null;
    }
}
