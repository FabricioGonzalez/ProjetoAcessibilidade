using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

namespace ProjectAvalonia.Common.Controls;

public class LabelsItemsPresenter
    : ItemsControl
{
    public static readonly StyledProperty<IBrush> ForegroundProperty =
        AvaloniaProperty.Register<LabelsItemsPresenter, IBrush>(name: "Foreground");

    public static readonly StyledProperty<IBrush> BorderBrushProperty =
        AvaloniaProperty.Register<LabelsItemsPresenter, IBrush>(name: "BorderBrush");

    public static readonly StyledProperty<double> MaxLabelWidthProperty =
        AvaloniaProperty.Register<LabelsItemsPresenter, double>(name: "MaxLabelWidth");

    public double MaxLabelWidth
    {
        get => GetValue(property: MaxLabelWidthProperty);
        set => SetValue(property: MaxLabelWidthProperty, value: value);
    }

    public IBrush Foreground
    {
        get => GetValue(property: ForegroundProperty);
        set => SetValue(property: ForegroundProperty, value: value);
    }

    public IBrush BorderBrush
    {
        get => GetValue(property: BorderBrushProperty);
        set => SetValue(property: BorderBrushProperty, value: value);
    }

    protected override Type StyleKeyOverride => typeof(LabelsItemsPresenter);

    /*protected override void PanelCreated(
        Panel panel
    )
    {
        base.PanelCreated(panel: panel);

        if (panel is LabelsPanel labelsPanel)
        {
            labelsPanel.WhenAnyValue(property1: x => x.VisibleItemsCount)
                .Subscribe(onNext: x =>
                {
                    if (Items is IEnumerable<string> items)
                    {
                        labelsPanel.FilteredItems = items.Skip(count: x).ToList();
                    }
                    else
                    {
                        labelsPanel.FilteredItems = new List<string>();
                    }
                });
        }
    }*/
}