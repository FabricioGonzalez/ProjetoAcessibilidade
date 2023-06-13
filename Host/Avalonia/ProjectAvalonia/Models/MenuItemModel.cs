using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Avalonia.Input;
using Avalonia.Media;

using ReactiveUI;

namespace ProjectAvalonia.Models;

public class MenuItemModel : ReactiveObject, IMenuItem
{
    public string Label
    {
        get; init;
    }
    public StreamGeometry Icon
    {
        get; init;
    }

    public ICommand Command
    {
        get; init;
    }

    public KeyGesture Gesture
    {
        get; init;
    }

    public IReadOnlyCollection<IMenuItem> Children
    {
        get; init;
    }


    public MenuItemModel(
         string label,
         ICommand? command,
         StreamGeometry? icon,
         string gesture = null,
         IEnumerable<IMenuItem>? children = null
        )
    {


        Children = children?.ToList() ?? Enumerable.Empty<IMenuItem>().ToList();
        Label = label;
        Icon = icon;
        Command = command;
        Gesture = KeyGesture.Parse(gesture);
    }
}