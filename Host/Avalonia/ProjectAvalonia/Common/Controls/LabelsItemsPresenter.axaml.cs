using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media;
using Avalonia.Styling;
using ReactiveUI;

namespace ProjectAvalonia.Common.Controls;

public class LabelsItemsPresenter
    : ItemsPresenter
        , IStyleable
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

    Type IStyleable.StyleKey => typeof(LabelsItemsPresenter);

    protected override void PanelCreated(
        IPanel panel
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
    }
}