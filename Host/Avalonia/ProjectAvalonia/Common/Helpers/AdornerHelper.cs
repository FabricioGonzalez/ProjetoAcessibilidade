using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ProjectAvalonia.Common.Helpers;

public class AdornerHelper
{
    public static void AddAdorner(
        Visual visual
        , Control adorner
    )
    {
        var layer = AdornerLayer.GetAdornerLayer(visual: visual);

        if (layer is not null && !layer.Children.Contains(item: adorner))
        {
            AdornerLayer.SetAdornedElement(adorner: adorner, adorned: visual);
            ((ISetLogicalParent)adorner).SetParent(parent: visual);
            layer.Children.Add(item: adorner);
        }
    }

    public static void RemoveAdorner(
        Visual visual
        , Control adorner
    )
    {
        var layer = AdornerLayer.GetAdornerLayer(visual: visual);

        if (layer is not null && layer.Children.Contains(item: adorner))
        {
            layer.Children.Remove(item: adorner);
            ((ISetLogicalParent)adorner).SetParent(parent: null);
        }
    }
}