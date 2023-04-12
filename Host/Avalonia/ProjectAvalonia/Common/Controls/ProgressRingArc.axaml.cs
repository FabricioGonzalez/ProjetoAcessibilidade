using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Controls;

public class ProgressRingArc : TemplatedControl
{
    public static readonly StyledProperty<IBrush> SegmentColorProperty =
        AvaloniaProperty.Register<ProgressRingArc, IBrush>(name: nameof(SegmentColor));

    public static readonly StyledProperty<int> StrokeThicknessProperty =
        AvaloniaProperty.Register<ProgressRingArc, int>(name: nameof(StrokeThickness), defaultValue: 5);

    public static readonly StyledProperty<double> PercentageProperty =
        AvaloniaProperty.Register<ProgressRingArc, double>(
            name: nameof(Percentage), defaultValue: 1);

    public static readonly DirectProperty<ProgressRingArc, int> PathFigureWidthProperty =
        AvaloniaProperty.RegisterDirect<ProgressRingArc, int>(
            name: nameof(PathFigureWidth),
            getter: o => o.PathFigureWidth,
            setter: (
                o
                , v
            ) => o.PathFigureWidth = v);

    public static readonly DirectProperty<ProgressRingArc, int> PathFigureHeightProperty =
        AvaloniaProperty.RegisterDirect<ProgressRingArc, int>(
            name: nameof(PathFigureHeight),
            getter: o => o.PathFigureHeight,
            setter: (
                o
                , v
            ) => o.PathFigureHeight = v);

    public static readonly DirectProperty<ProgressRingArc, Thickness> PathFigureMarginProperty =
        AvaloniaProperty.RegisterDirect<ProgressRingArc, Thickness>(
            name: nameof(PathFigureMargin),
            getter: o => o.PathFigureMargin,
            setter: (
                o
                , v
            ) => o.PathFigureMargin = v);

    public static readonly DirectProperty<ProgressRingArc, Point> PathFigureStartPointProperty =
        AvaloniaProperty.RegisterDirect<ProgressRingArc, Point>(
            name: nameof(PathFigureStartPoint),
            getter: o => o.PathFigureStartPoint,
            setter: (
                o
                , v
            ) => o.PathFigureStartPoint = v);

    public static readonly DirectProperty<ProgressRingArc, Point> ArcSegmentPointProperty =
        AvaloniaProperty.RegisterDirect<ProgressRingArc, Point>(
            name: nameof(ArcSegmentPoint),
            getter: o => o.ArcSegmentPoint,
            setter: (
                o
                , v
            ) => o.ArcSegmentPoint = v);

    public static readonly DirectProperty<ProgressRingArc, Size> ArcSegmentSizeProperty =
        AvaloniaProperty.RegisterDirect<ProgressRingArc, Size>(
            name: nameof(ArcSegmentSize),
            getter: o => o.ArcSegmentSize,
            setter: (
                o
                , v
            ) => o.ArcSegmentSize = v);

    public static readonly DirectProperty<ProgressRingArc, bool> ArcSegmentIsLargeArcProperty =
        AvaloniaProperty.RegisterDirect<ProgressRingArc, bool>(
            name: nameof(ArcSegmentIsLargeArc),
            getter: o => o.ArcSegmentIsLargeArc,
            setter: (
                o
                , v
            ) => o.ArcSegmentIsLargeArc = v);

    private bool _arcSegmentIsLargeArc;
    private Point _arcSegmentPoint;
    private Size _arcSegmentSize;
    private int _pathFigureHeight;
    private Thickness _pathFigureMargin;
    private Point _pathFigureStartPoint;
    private int _pathFigureWidth;
    private double _radius;

    public IBrush SegmentColor
    {
        get => GetValue(property: SegmentColorProperty);
        set => SetValue(property: SegmentColorProperty, value: value);
    }

    public int StrokeThickness
    {
        get => GetValue(property: StrokeThicknessProperty);
        set => SetValue(property: StrokeThicknessProperty, value: value);
    }

    public double Percentage
    {
        get => GetValue(property: PercentageProperty);
        set => SetValue(property: PercentageProperty, value: value);
    }

    public int PathFigureWidth
    {
        get => _pathFigureWidth;
        private set => SetAndRaise(property: PathFigureWidthProperty, field: ref _pathFigureWidth, value: value);
    }

    public int PathFigureHeight
    {
        get => _pathFigureHeight;
        private set => SetAndRaise(property: PathFigureHeightProperty, field: ref _pathFigureHeight, value: value);
    }

    public Thickness PathFigureMargin
    {
        get => _pathFigureMargin;
        private set => SetAndRaise(property: PathFigureMarginProperty, field: ref _pathFigureMargin, value: value);
    }

    public Point PathFigureStartPoint
    {
        get => _pathFigureStartPoint;
        private set => SetAndRaise(property: PathFigureStartPointProperty, field: ref _pathFigureStartPoint
            , value: value);
    }

    public Point ArcSegmentPoint
    {
        get => _arcSegmentPoint;
        private set => SetAndRaise(property: ArcSegmentPointProperty, field: ref _arcSegmentPoint, value: value);
    }

    public Size ArcSegmentSize
    {
        get => _arcSegmentSize;
        private set => SetAndRaise(property: ArcSegmentSizeProperty, field: ref _arcSegmentSize, value: value);
    }

    public bool ArcSegmentIsLargeArc
    {
        get => _arcSegmentIsLargeArc;
        private set => SetAndRaise(property: ArcSegmentIsLargeArcProperty, field: ref _arcSegmentIsLargeArc
            , value: value);
    }

    protected override void OnPropertyChanged<T>(
        AvaloniaPropertyChangedEventArgs<T> e
    )
    {
        base.OnPropertyChanged(change: e);

        if (e.Property == SegmentColorProperty ||
            e.Property == StrokeThicknessProperty ||
            e.Property == PercentageProperty)
        {
            RenderArc();
        }
    }

    protected override Size MeasureOverride(
        Size availableSize
    )
    {
        _radius = availableSize.Height / 2;
        _radius -= StrokeThickness;
        RenderArc();
        return new Size(width: _radius * 2, height: _radius * 2);
    }

    private void RenderArc()
    {
        var angle = Percentage * 360;

        var startPoint = new Point(x: _radius, y: 0);
        var endPoint = ComputeCartesianCoordinate(angle: angle, radius: _radius);
        endPoint += new Point(x: _radius, y: _radius);

        PathFigureWidth = (int)_radius * 2 + StrokeThickness;
        PathFigureHeight = (int)_radius * 2 + StrokeThickness;
        PathFigureMargin = new Thickness(left: StrokeThickness, top: StrokeThickness, right: 0, bottom: 0);

        var largeArc = angle > 180.0;

        var outerArcSize = new Size(width: _radius, height: _radius);

        PathFigureStartPoint = startPoint;

        if (Math.Abs(value: startPoint.X - Math.Round(a: endPoint.X)) < 0.01 &&
            Math.Abs(value: startPoint.Y - Math.Round(a: endPoint.Y)) < 0.01)
        {
            endPoint -= new Point(x: 0.01, y: 0);
        }

        ArcSegmentPoint = endPoint;
        ArcSegmentSize = outerArcSize;
        ArcSegmentIsLargeArc = largeArc;
    }

    private static Point ComputeCartesianCoordinate(
        double angle
        , double radius
    )
    {
        // convert to radians
        var angleRad = Math.PI / 180.0 * (angle - 90);

        var x = radius * Math.Cos(d: angleRad);
        var y = radius * Math.Sin(a: angleRad);

        return new Point(x: x, y: y);
    }
}