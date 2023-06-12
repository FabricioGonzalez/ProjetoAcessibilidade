using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Avalonia.Media;

using ReactiveUI;

namespace ProjectAvalonia.Models;

public class MenuItemModel : ReactiveObject, IMenuItem
{
    public string Label => _menuItem.Label;
    public DrawingGroup Icon => _menuItem.Icon;

    public ICommand Command => _menuItem.Command;

    public string Gesture => _menuItem?.Gesture;

    public IEnumerable<IMenuItem> Children
    {
        get;
    }

    private IMenuItem _menuItem;

    public MenuItemModel(
        IMenuItem menuItem, IEnumerable<IMenuItem>? children = null)
    {
        _menuItem = menuItem;

        Children = children?.ToList() ?? Enumerable.Empty<IMenuItem>();
    }
}