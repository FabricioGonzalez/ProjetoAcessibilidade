using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace ProjectAvalonia.Common.Controls;

public class LabelsPanel : VirtualizingStackPanel
{
    public static readonly StyledProperty<Control?> EllipsisControlProperty =
        AvaloniaProperty.Register<LabelsPanel, Control?>(name: nameof(EllipsisControl));

    public static readonly DirectProperty<LabelsPanel, int> VisibleItemsCountProperty =
        AvaloniaProperty.RegisterDirect<LabelsPanel, int>(
            name: nameof(VisibleItemsCount),
            getter: o => o.VisibleItemsCount);

    private int _visibleItemsCount;

    public Control? EllipsisControl
    {
        get => GetValue(property: EllipsisControlProperty);
        set => SetValue(property: EllipsisControlProperty, value: value);
    }

    public int VisibleItemsCount
    {
        get => _visibleItemsCount;
        private set => SetAndRaise(property: VisibleItemsCountProperty, field: ref _visibleItemsCount, value: value);
    }

    public List<string>? FilteredItems
    {
        get;
        set;
    }

    public double Spacing
    {
        get;
        set;
    }

    protected override void OnAttachedToVisualTree(
        VisualTreeAttachmentEventArgs e
    )
    {
        if (EllipsisControl is { } ellipsisControl)
        {
            ((ISetLogicalParent)ellipsisControl).SetParent(parent: this);
            VisualChildren.Add(item: ellipsisControl);
            LogicalChildren.Add(item: ellipsisControl);
        }

        base.OnAttachedToVisualTree(e: e);
    }

    protected override void OnDetachedFromVisualTree(
        VisualTreeAttachmentEventArgs e
    )
    {
        if (EllipsisControl is { } ellipsisControl)
        {
            ((ISetLogicalParent)ellipsisControl).SetParent(parent: null);
            LogicalChildren.Remove(item: ellipsisControl);
            VisualChildren.Remove(item: ellipsisControl);
        }

        base.OnDetachedFromVisualTree(e: e);
    }

    protected override Size MeasureOverride(
        Size availableSize
    )
    {
        var ellipsis = 0.0;
        if (EllipsisControl is not null)
        {
            EllipsisControl.Measure(availableSize: availableSize);
            ellipsis = EllipsisControl.DesiredSize.Width;
        }

        return base.MeasureOverride(availableSize: availableSize.WithWidth(width: availableSize.Width + ellipsis));
    }

    protected override Size ArrangeOverride(
        Size finalSize
    )
    {
        var spacing = Spacing;
        var ellipsisWidth = 0.0;
        var width = 0.0;
        var height = 0.0;
        var finalWidth = finalSize.Width;
        var showEllipsis = false;
        var totalChildren = Children.Count;
        var count = 0;

        if (EllipsisControl is not null)
        {
            ellipsisWidth = EllipsisControl.DesiredSize.Width;
        }

        for (var i = 0; i < totalChildren; i++)
        {
            var child = Children[index: i];
            var childWidth = child.DesiredSize.Width;

            height = Math.Max(val1: height, val2: child.DesiredSize.Height);

            if (width + childWidth > finalWidth)
            {
                while (true)
                {
                    if (width + ellipsisWidth > finalWidth)
                    {
                        var previous = i - 1;
                        if (previous >= 0)
                        {
                            var previousChild = Children[index: previous];
                            count--;
                            width -= previousChild.DesiredSize.Width + spacing;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                showEllipsis = true;
                if (EllipsisControl is not null)
                {
                    width += EllipsisControl.DesiredSize.Width;
                }

                break;
            }

            width += child.DesiredSize.Width + spacing;
            count++;
        }

        var offset = 0.0;

        for (var i = 0; i < totalChildren; i++)
        {
            var child = Children[index: i];
            if (i < count)
            {
                var rect = new Rect(x: offset, y: 0.0, width: child.DesiredSize.Width, height: height);
                child.Arrange(rect: rect);
                offset += child.DesiredSize.Width + spacing;
            }
            else
            {
                child.Arrange(rect: new Rect(x: -10000, y: -10000, width: 0, height: 0));
            }
        }

        if (EllipsisControl is not null)
        {
            if (showEllipsis)
            {
                var rect = new Rect(x: offset, y: 0.0, width: EllipsisControl.DesiredSize.Width, height: height);
                EllipsisControl.Arrange(rect: rect);
            }
            else
            {
                EllipsisControl.Arrange(rect: new Rect(x: -10000, y: -10000, width: 0, height: 0));
            }
        }

        VisibleItemsCount = count;

        return new Size(width: width, height: height);
    }
}