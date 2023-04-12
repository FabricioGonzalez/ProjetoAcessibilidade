using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Utilities;

namespace ProjectAvalonia.Common.Controls;

/// <summary>
///     Works like a wrap panel.. but concatenates the Items in ConcatenatedChildren.
///     Also the very last child in ConcatenatedChildren will fill the remaining space.
/// </summary>
public class ConcatenatingWrapPanel
    : Panel
        , INavigableContainer
{
    /// <summary>
    ///     Defines the <see cref="Orientation" /> property.
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<ConcatenatingWrapPanel, Orientation>(name: nameof(Orientation)
            , defaultValue: Orientation.Horizontal);

    /// <summary>
    ///     Defines the <see cref="ItemWidth" /> property.
    /// </summary>
    public static readonly StyledProperty<double> ItemWidthProperty =
        AvaloniaProperty.Register<ConcatenatingWrapPanel, double>(name: nameof(ItemWidth), defaultValue: double.NaN);

    /// <summary>
    ///     Defines the <see cref="ItemHeight" /> property.
    /// </summary>
    public static readonly StyledProperty<double> ItemHeightProperty =
        AvaloniaProperty.Register<ConcatenatingWrapPanel, double>(name: nameof(ItemHeight), defaultValue: double.NaN);

    /// <summary>
    ///     Initializes static members of the <see cref="ConcatenatingWrapPanel" /> class.
    /// </summary>
    static ConcatenatingWrapPanel()
    {
        AffectsMeasure<ConcatenatingWrapPanel>(OrientationProperty, ItemWidthProperty, ItemHeightProperty);
    }

    public ConcatenatingWrapPanel()
    {
        ConcatenatedChildren.CollectionChanged += ChildrenChanged;
    }

    public Avalonia.Controls.Controls ConcatenatedChildren
    {
        get;
    } = new();

    /// <summary>
    ///     Gets or sets the orientation in which child controls will be layed out.
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(property: OrientationProperty);
        set => SetValue(property: OrientationProperty, value: value);
    }

    /// <summary>
    ///     Gets or sets the width of all items in the WrapPanel.
    /// </summary>
    public double ItemWidth
    {
        get => GetValue(property: ItemWidthProperty);
        set => SetValue(property: ItemWidthProperty, value: value);
    }

    /// <summary>
    ///     Gets or sets the height of all items in the WrapPanel.
    /// </summary>
    public double ItemHeight
    {
        get => GetValue(property: ItemHeightProperty);
        set => SetValue(property: ItemHeightProperty, value: value);
    }

    /// <summary>
    ///     Gets the next control in the specified direction.
    /// </summary>
    /// <param name="direction">The movement direction.</param>
    /// <param name="from">The control from which movement begins.</param>
    /// <param name="wrap">Whether to wrap around when the first or last item is reached.</param>
    /// <returns>The control.</returns>
    IInputElement INavigableContainer.GetControl(
        NavigationDirection direction
        , IInputElement from
        , bool wrap
    )
    {
        var orientation = Orientation;
        var children = Children.Concat(second: ConcatenatedChildren).ToList();
        var horiz = orientation == Orientation.Horizontal;
        var index = Children.IndexOf(item: (IControl)from);

        switch (direction)
        {
            case NavigationDirection.First:
                index = 0;
                break;

            case NavigationDirection.Last:
                index = children.Count - 1;
                break;

            case NavigationDirection.Next:
                ++index;
                break;

            case NavigationDirection.Previous:
                --index;
                break;

            case NavigationDirection.Left:
                index = horiz ? index - 1 : -1;
                break;

            case NavigationDirection.Right:
                index = horiz ? index + 1 : -1;
                break;

            case NavigationDirection.Up:
                index = horiz ? -1 : index - 1;
                break;

            case NavigationDirection.Down:
                index = horiz ? -1 : index + 1;
                break;
        }

        if (index >= 0 && index < children.Count)
        {
            return children[index: index];
        }

        return null!;
    }

    /// <inheritdoc />
    protected override Size MeasureOverride(
        Size constraint
    )
    {
        var itemWidth = ItemWidth;
        var itemHeight = ItemHeight;
        var orientation = Orientation;
        var children = Children.Concat(second: ConcatenatedChildren).ToList();
        var curLineSize = new UVSize(orientation: orientation);
        var panelSize = new UVSize(orientation: orientation);
        var uvConstraint = new UVSize(orientation: orientation, width: constraint.Width, height: constraint.Height);
        var itemWidthSet = !double.IsNaN(d: itemWidth);
        var itemHeightSet = !double.IsNaN(d: itemHeight);

        var childConstraint = new Size(
            width: itemWidthSet ? itemWidth : constraint.Width,
            height: itemHeightSet ? itemHeight : constraint.Height);

        for (int i = 0, count = children.Count; i < count; i++)
        {
            var child = children[index: i];
            if (child is not null)
            {
                // Flow passes its own constraint to children
                child.Measure(availableSize: childConstraint);

                // This is the size of the child in UV space
                var sz = new UVSize(
                    orientation: orientation,
                    width: itemWidthSet ? itemWidth : child.DesiredSize.Width,
                    height: itemHeightSet ? itemHeight : child.DesiredSize.Height);

                if (MathUtilities.GreaterThan(value1: curLineSize.U + sz.U
                        , value2: uvConstraint.U)) // Need to switch to another line
                {
                    panelSize.U = Math.Max(val1: curLineSize.U, val2: panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;

                    if (MathUtilities.GreaterThan(value1: sz.U
                            , value2: uvConstraint
                                .U)) // The element is wider then the constraint - give it a separate line
                    {
                        panelSize.U = Math.Max(val1: sz.U, val2: panelSize.U);
                        panelSize.V += sz.V;
                        curLineSize = new UVSize(orientation: orientation);
                    }
                }
                else // Continue to accumulate a line
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(val1: sz.V, val2: curLineSize.V);
                }
            }
        }

        // The last line size, if any should be added
        panelSize.U = Math.Max(val1: curLineSize.U, val2: panelSize.U);
        panelSize.V += curLineSize.V;

        // Go from UV space to W/H space
        return new Size(width: panelSize.Width, height: panelSize.Height);
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(
        Size finalSize
    )
    {
        var itemWidth = ItemWidth;
        var itemHeight = ItemHeight;
        var orientation = Orientation;
        var children = Children.Concat(second: ConcatenatedChildren).ToList();
        var firstInLine = 0;
        double accumulatedV = 0;
        var itemU = orientation == Orientation.Horizontal ? itemWidth : itemHeight;
        var curLineSize = new UVSize(orientation: orientation);
        var uvFinalSize = new UVSize(orientation: orientation, width: finalSize.Width, height: finalSize.Height);
        var itemWidthSet = !double.IsNaN(d: itemWidth);
        var itemHeightSet = !double.IsNaN(d: itemHeight);
        var useItemU = orientation == Orientation.Horizontal ? itemWidthSet : itemHeightSet;
        var hasWrapped = false;

        for (var i = 0; i < children.Count; i++)
        {
            var child = children[index: i];
            if (child is not null)
            {
                var sz = new UVSize(
                    orientation: orientation,
                    width: itemWidthSet ? itemWidth : child.DesiredSize.Width,
                    height: itemHeightSet ? itemHeight : child.DesiredSize.Height);

                if (MathUtilities.GreaterThan(value1: curLineSize.U + sz.U
                        , value2: uvFinalSize.U)) // Need to switch to another line
                {
                    hasWrapped = true;
                    ArrangeLine(v: accumulatedV, lineV: curLineSize.V, start: firstInLine, end: i, useItemU: useItemU
                        , itemU: itemU, uvFinalSize: uvFinalSize);

                    accumulatedV += curLineSize.V;
                    curLineSize = sz;

                    if (MathUtilities.GreaterThan(value1: sz.U
                            , value2: uvFinalSize
                                .U)) // The element is wider then the constraint - give it a separate line
                    {
                        // Switch to next line which only contain one element
                        ArrangeLine(v: accumulatedV, lineV: sz.V, start: i, end: ++i, useItemU: useItemU, itemU: itemU
                            , uvFinalSize: uvFinalSize);

                        accumulatedV += sz.V;
                        curLineSize = new UVSize(orientation: orientation);
                    }

                    firstInLine = i;
                }
                else // Continue to accumulate a line
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(val1: sz.V, val2: curLineSize.V);
                }
            }
        }

        // Arrange the last line, if any
        if (firstInLine < children.Count)
        {
            ArrangeLine(v: accumulatedV, lineV: curLineSize.V, start: firstInLine, end: children.Count
                , useItemU: useItemU, itemU: itemU, uvFinalSize: uvFinalSize);
        }

        PseudoClasses.Set(name: ":wrapped", value: hasWrapped);

        return finalSize;
    }

    private void ArrangeLine(
        double v
        , double lineV
        , int start
        , int end
        , bool useItemU
        , double itemU
        , UVSize uvFinalSize
    )
    {
        var orientation = Orientation;
        var children = Children.Concat(second: ConcatenatedChildren).ToList();
        double u = 0;
        var isHorizontal = orientation == Orientation.Horizontal;

        for (var i = start; i < end; i++)
        {
            var child = children[index: i];
            if (child is not null)
            {
                if (i == children.Count - 1)
                {
                    var childSize = new UVSize(orientation: orientation, width: uvFinalSize.Width - u
                        , height: uvFinalSize.Height - u);
                    var layoutSlotU = useItemU ? itemU : childSize.U;
                    child.Arrange(rect: new Rect(
                        x: isHorizontal ? u : v,
                        y: isHorizontal ? v : u,
                        width: isHorizontal ? layoutSlotU : lineV,
                        height: isHorizontal ? lineV : layoutSlotU));
                    u += layoutSlotU;
                }
                else
                {
                    var childSize = new UVSize(orientation: orientation, width: child.DesiredSize.Width
                        , height: child.DesiredSize.Height);
                    var layoutSlotU = useItemU ? itemU : childSize.U;
                    child.Arrange(rect: new Rect(
                        x: isHorizontal ? u : v,
                        y: isHorizontal ? v : u,
                        width: isHorizontal ? layoutSlotU : lineV,
                        height: isHorizontal ? lineV : layoutSlotU));
                    u += layoutSlotU;
                }
            }
        }
    }

    private struct UVSize
    {
        internal double U;
        internal double V;
        private readonly Orientation _orientation;

        internal UVSize(
            Orientation orientation
            , double width
            , double height
        )
        {
            U = V = 0d;
            _orientation = orientation;
            Width = width;
            Height = height;
        }

        internal UVSize(
            Orientation orientation
        )
        {
            U = V = 0d;
            _orientation = orientation;
        }

        internal double Width
        {
            get => _orientation == Orientation.Horizontal ? U : V;
            set
            {
                if (_orientation == Orientation.Horizontal)
                {
                    U = value;
                }
                else
                {
                    V = value;
                }
            }
        }

        internal double Height
        {
            get => _orientation == Orientation.Horizontal ? V : U;
            set
            {
                if (_orientation == Orientation.Horizontal)
                {
                    V = value;
                }
                else
                {
                    U = value;
                }
            }
        }
    }
}