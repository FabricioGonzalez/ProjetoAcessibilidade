using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ProjectAvalonia.Common.Controls;

public class HistoryPlaceholderPanel : ContentControl
{
    private ItemsControl? _targetItemsControl;

    public double RowHeight
    {
        get;
        set;
    } = 36.5;

    protected override void OnApplyTemplate(
        TemplateAppliedEventArgs e
    )
    {
        _targetItemsControl = e.NameScope.Find<ItemsControl>(name: "PART_DummyRows");
        InvalidateMeasure();
        base.OnApplyTemplate(e: e);
    }

    protected override Size MeasureOverride(
        Size availableSize
    )
    {
        if (_targetItemsControl is null)
        {
            return availableSize;
        }

        var totalRows = (int)Math.Floor(d: Math.Max(val1: 1, val2: availableSize.Height / RowHeight));

        var deltaOpacity = 1d / totalRows;

        _targetItemsControl.Items =
            Enumerable
                .Range(start: 1, count: totalRows)
                .Reverse()
                .Select(selector: mult => mult * deltaOpacity)
                .ToList();

        return base.MeasureOverride(availableSize: availableSize);
    }
}