using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MathNet.Numerics.Interpolation;
using ProjectAvalonia.Common.Helpers;

namespace ProjectAvalonia.Common.Controls;

public partial class LineChart : Control
{
    private readonly Dictionary<INotifyCollectionChanged, IDisposable> _collectionChangedSubscriptions;

    public LineChart()
    {
        _collectionChangedSubscriptions = new Dictionary<INotifyCollectionChanged, IDisposable>();

        AddHandler(routedEvent: PointerPressedEvent, handler: PointerPressedHandler, routes: RoutingStrategies.Tunnel);
        AddHandler(routedEvent: PointerReleasedEvent, handler: PointerReleasedHandler
            , routes: RoutingStrategies.Tunnel);
        AddHandler(routedEvent: PointerMovedEvent, handler: PointerMovedHandler, routes: RoutingStrategies.Tunnel);
        AddHandler(routedEvent: PointerExitedEvent, handler: PointerLeaveHandler, routes: RoutingStrategies.Tunnel);
    }

    private static double Clamp(
        double val
        , double min
        , double max
    ) => Math.Min(val1: Math.Max(val1: val, val2: min), val2: max);

    private static Geometry CreateFillGeometry(
        IReadOnlyList<Point> points
        , double width
        , double height
    )
    {
        var geometry = new StreamGeometry();
        using var context = geometry.Open();
        context.BeginFigure(startPoint: points[0], isFilled: true);
        for (var i = 1; i < points.Count; i++)
        {
            context.LineTo(points[i]);
        }

        context.LineTo(new Point(x: width, y: height));
        context.LineTo(new Point(x: points[0].X, y: height));
        context.EndFigure(true);
        return geometry;
    }

    private static Geometry CreateStrokeGeometry(
        IReadOnlyList<Point> points
    )
    {
        var geometry = new StreamGeometry();
        using var context = geometry.Open();
        context.BeginFigure(startPoint: points[0], isFilled: false);
        for (var i = 1; i < points.Count; i++)
        {
            context.LineTo(points[i]);
        }

        context.EndFigure(false);
        return geometry;
    }

    private static FormattedText CreateFormattedText(
        string text
        , Typeface typeface
        /*, TextAlignment alignment*/
        , double fontSize
        /*, Size constraint*/
    ) =>
        new(
            textToFormat: text,
            culture: CultureInfo.CurrentUICulture,
            flowDirection: FlowDirection.LeftToRight,
            typeface: typeface,
            emSize: fontSize,
            foreground: null
        );

    private void UpdateXAxisCursorPosition(
        double x
    )
    {
        var xAxisValues = XAxisValues;
        if (xAxisValues is null || xAxisValues.Count == 0)
        {
            XAxisCurrentValue = double.NaN;
            return;
        }

        var areaWidth = Bounds.Width - AreaMargin.Left - AreaMargin.Right;
        var value = Clamp(val: x - AreaMargin.Left, min: 0, max: areaWidth);
        var factor = value / areaWidth;
        var index = (int)((xAxisValues.Count - 1) * factor);
        var currentValue = xAxisValues[index];
        XAxisCurrentValue = currentValue;
    }

    private Rect? GetXAxisCursorHitTestRect()
    {
        var chartWidth = Bounds.Width;
        var chartHeight = Bounds.Height;
        var areaMargin = AreaMargin;
        var areaWidth = chartWidth - areaMargin.Left - areaMargin.Right;
        var areaHeight = chartHeight - areaMargin.Top - areaMargin.Bottom;
        var areaRect = new Rect(x: areaMargin.Left, y: areaMargin.Top, width: areaWidth, height: areaHeight);
        var cursorPosition = GetCursorPosition(areaWidth);
        if (double.IsNaN(cursorPosition))
        {
            return null;
        }

        var cursorHitTestSize = 5;
        var cursorStrokeThickness = CursorStrokeThickness;
        var cursorHitTestRect = new Rect(
            x: areaMargin.Left + cursorPosition - cursorHitTestSize + cursorStrokeThickness / 2,
            y: areaRect.Top,
            width: cursorHitTestSize + cursorHitTestSize,
            height: areaRect.Height);
        return cursorHitTestRect;
    }

    private void PointerLeaveHandler(
        object? sender
        , PointerEventArgs e
    ) => Cursor = new Cursor(StandardCursorType.Arrow);

    private void PointerMovedHandler(
        object? sender
        , PointerEventArgs e
    )
    {
        var position = e.GetPosition(this);
        if (_captured)
        {
            UpdateXAxisCursorPosition(position.X);
        }
        else
        {
            if (CursorStroke is null)
            {
                return;
            }

            var cursorHitTestRect = GetXAxisCursorHitTestRect();
            var cursorSizeWestEast = cursorHitTestRect is not null && cursorHitTestRect.Value.Contains(position);
            Cursor = cursorSizeWestEast
                ? new Cursor(StandardCursorType.SizeWestEast)
                : new Cursor(StandardCursorType.Arrow);
        }
    }

    private void PointerReleasedHandler(
        object? sender
        , PointerReleasedEventArgs e
    )
    {
        if (!_captured)
        {
            return;
        }

        var position = e.GetPosition(this);
        var cursorHitTestRect = GetXAxisCursorHitTestRect();
        var cursorSizeWestEast = cursorHitTestRect is not null && cursorHitTestRect.Value.Contains(position);
        if (!cursorSizeWestEast)
        {
            Cursor = new Cursor(StandardCursorType.Arrow);
        }

        _captured = false;
    }

    private void PointerPressedHandler(
        object? sender
        , PointerPressedEventArgs e
    )
    {
        var position = e.GetPosition(this);
        UpdateXAxisCursorPosition(position.X);
        Cursor = new Cursor(StandardCursorType.SizeWestEast);
        _captured = true;
    }

    private LineChartState CreateChartState(
        double width
        , double height
    )
    {
        var state = new LineChartState
        {
            ChartWidth = width, ChartHeight = height, AreaMargin = AreaMargin
        };

        state.AreaWidth = width - state.AreaMargin.Left - state.AreaMargin.Right;
        state.AreaHeight = height - state.AreaMargin.Top - state.AreaMargin.Bottom;

        SetStateAreaPoints(state);

        SetStateXAxisLabels(state);
        SetStateYAxisLabels(state);

        SetStateXAxisCursor(state);

        return state;
    }

    private static IEnumerable<(double x, double y)> SplineInterpolate(
        double[] xs
        , double[] ys
    )
    {
        const int Divisions = 256;

        if (xs.Length > 2)
        {
            var spline = CubicSpline.InterpolatePchipSorted(x: xs, y: ys);

            for (var i = 0; i < xs.Length - 1; i++)
            {
                var a = xs[i];
                var b = xs[i + 1];
                var range = b - a;
                var step = range / Divisions;

                var t0 = xs[i];

                var xt0 = spline.Interpolate(xs[i]);

                yield return (t0, xt0);

                for (var t = a + step; t < b; t += step)
                {
                    var xt = spline.Interpolate(t);

                    yield return (t, xt);
                }
            }

            var tn = xs[^1];
            var xtn = spline.Interpolate(xs[^1]);

            yield return (tn, xtn);
        }
        else
        {
            for (var i = 0; i < xs.Length; i++)
            {
                yield return (xs[i], ys[i]);
            }
        }
    }

    private void SetStateAreaPoints(
        LineChartState state
    )
    {
        var xAxisValues = XAxisValues;
        var yAxisValues = YAxisValues;

        if (xAxisValues is null || xAxisValues.Count <= 1 || yAxisValues is null || yAxisValues.Count <= 1)
        {
            state.XAxisLabelStep = double.NaN;
            state.YAxisLabelStep = double.NaN;
            state.Points = null;
            return;
        }

        var logarithmicScale = YAxisLogarithmicScale;

        var yAxisValuesLogScaled = logarithmicScale
            ? yAxisValues.Select(y => Math.Log(y)).ToList()
            : yAxisValues.ToList();

        var yAxisValuesLogScaledMax = yAxisValuesLogScaled.Max();

        var yAxisScaler = new StraightLineFormula();
        yAxisScaler.CalculateFrom(x1: yAxisValuesLogScaledMax, x2: 0, y1: 0, y2: state.AreaHeight);

        var yAxisValuesScaled = yAxisValuesLogScaled
            .Select(y => yAxisScaler.GetYforX(y))
            .ToList();

        var xAxisValuesEnumerable = xAxisValues as IEnumerable<double>;

        switch (XAxisPlotMode)
        {
            case AxisPlotMode.Normal:
                var min = XAxisMinimum ?? xAxisValues.Min();
                var max = xAxisValues.Max();

                var xAxisScaler = new StraightLineFormula();
                xAxisScaler.CalculateFrom(x1: min, x2: max, y1: 0, y2: state.AreaWidth);

                xAxisValuesEnumerable = xAxisValuesEnumerable.Select(x => xAxisScaler.GetYforX(x));
                break;

            case AxisPlotMode.EvenlySpaced:
                var pointStep = state.AreaWidth / (xAxisValues.Count - 1);

                xAxisValuesEnumerable = Enumerable.Range(start: 0, count: xAxisValues.Count).Select(x => pointStep * x);
                break;

            case AxisPlotMode.Logarithmic:
                break;
        }

        if (SmoothCurve)
        {
            var interpolated = SplineInterpolate(xs: xAxisValuesEnumerable.ToArray(), ys: yAxisValuesScaled.ToArray());

            state.Points = interpolated.Select(pt => new Point(x: pt.x, y: pt.y)).ToArray();
        }
        else
        {
            state.Points = new Point[xAxisValues.Count];

            using var enumerator = xAxisValuesEnumerable.GetEnumerator();
            for (var i = 0; i < yAxisValuesScaled.Count; i++)
            {
                enumerator.MoveNext();
                state.Points[i] = new Point(x: enumerator.Current, y: yAxisValuesScaled[i]);
            }
        }
    }

    private void SetStateXAxisLabels(
        LineChartState state
    )
    {
        var xAxisLabels = XAxisLabels;

        if (xAxisLabels is not null)
        {
            state.XAxisLabelStep = xAxisLabels.Count <= 1
                ? double.NaN
                : state.AreaWidth / (xAxisLabels.Count - 1);

            state.XAxisLabels = xAxisLabels.ToList();
        }
        else
        {
            AutoGenerateXAxisLabels(state);
        }
    }

    private void SetStateYAxisLabels(
        LineChartState state
    )
    {
        var yAxisLabels = YAxisLabels;

        if (yAxisLabels is not null)
        {
            state.YAxisLabelStep = yAxisLabels.Count <= 1
                ? double.NaN
                : state.AreaHeight / (yAxisLabels.Count - 1);

            state.YAxisLabels = yAxisLabels.ToList();
        }
        else
        {
            AutoGenerateYAxisLabels(state);
        }
    }

    private void AutoGenerateXAxisLabels(
        LineChartState state
    )
    {
        var xAxisValues = XAxisValues;

        state.XAxisLabelStep = xAxisValues is null || xAxisValues.Count <= 1
            ? double.NaN
            : state.AreaWidth / (xAxisValues.Count - 1);

        if (XAxisStroke is not null && XAxisValues is not null)
        {
            state.XAxisLabels = XAxisValues.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
        }
    }

    private void AutoGenerateYAxisLabels(
        LineChartState state
    )
    {
        var yAxisValues = YAxisValues;

        state.YAxisLabelStep = yAxisValues is null || yAxisValues.Count <= 1
            ? double.NaN
            : state.AreaHeight / (yAxisValues.Count - 1);

        if (YAxisStroke is not null && YAxisValues is not null)
        {
            state.YAxisLabels = YAxisValues.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
        }
    }

    private double GetCursorPosition(
        double areaWidth
    )
    {
        var xAxisCurrentValue = XAxisCurrentValue;
        var xAxisValues = XAxisValues;
        if (double.IsNaN(xAxisCurrentValue) || xAxisValues is null || xAxisValues.Count == 0)
        {
            return double.NaN;
        }

        for (var i = 0; i < xAxisValues.Count; i++)
        {
            if (xAxisValues[i] <= xAxisCurrentValue)
            {
                var cursorPosition = i == 0 ? 0.0 : areaWidth / (xAxisValues.Count - 1) * i;
                return cursorPosition;
            }
        }

        return double.NaN;
    }

    private void SetStateXAxisCursor(
        LineChartState state
    ) => state.XAxisCursorPosition = GetCursorPosition(state.AreaWidth);

    private void DrawAreaFill(
        DrawingContext context
        , LineChartState state
    )
    {
        var brush = AreaFill;
        if (brush is null
            || state.Points is null
            || state.AreaWidth <= 0
            || state.AreaHeight <= 0
            || state.AreaWidth < AreaMinViableWidth
            || state.AreaHeight < AreaMinViableHeight)
        {
            return;
        }

        var deflate = 0.5;
        var geometry = CreateFillGeometry(points: state.Points, width: state.AreaWidth, height: state.AreaHeight);
        var transform = context.PushPreTransform(
            Matrix.CreateTranslation(
                xPosition: state.AreaMargin.Left + deflate,
                yPosition: state.AreaMargin.Top + deflate));
        context.DrawGeometry(brush: brush, pen: null, geometry: geometry);
        transform.Dispose();
    }

    private void DrawAreaStroke(
        DrawingContext context
        , LineChartState state
    )
    {
        var brush = AreaStroke;
        if (brush is null
            || state.Points is null
            || state.AreaWidth <= 0
            || state.AreaHeight <= 0
            || state.AreaWidth < AreaMinViableWidth
            || state.AreaHeight < AreaMinViableHeight)
        {
            return;
        }

        var thickness = AreaStrokeThickness;
        var dashStyle = AreaStrokeDashStyle;
        var lineCap = AreaStrokeLineCap;
        var lineJoin = AreaStrokeLineJoin;
        var miterLimit = AreaStrokeMiterLimit;
        var pen = new Pen(brush: brush, thickness: thickness, dashStyle: dashStyle, lineCap: lineCap, lineJoin: lineJoin
            , miterLimit: miterLimit);
        var deflate = thickness * 0.5;
        var geometry = CreateStrokeGeometry(state.Points);
        var transform = context.PushPreTransform(
            Matrix.CreateTranslation(
                xPosition: state.AreaMargin.Left + deflate,
                yPosition: state.AreaMargin.Top + deflate));
        context.DrawGeometry(brush: null, pen: pen, geometry: geometry);
        transform.Dispose();
    }

    private void DrawCursor(
        DrawingContext context
        , LineChartState state
    )
    {
        var brush = CursorStroke;
        if (brush is null
            || double.IsNaN(state.XAxisCursorPosition)
            || state.AreaWidth <= 0
            || state.AreaHeight <= 0
            || state.AreaWidth < AreaMinViableWidth
            || state.AreaHeight < AreaMinViableHeight)
        {
            return;
        }

        var thickness = CursorStrokeThickness;
        var dashStyle = CursorStrokeDashStyle;
        var lineCap = CursorStrokeLineCap;
        var lineJoin = CursorStrokeLineJoin;
        var miterLimit = CursorStrokeMiterLimit;
        var pen = new Pen(brush: brush, thickness: thickness, dashStyle: dashStyle, lineCap: lineCap, lineJoin: lineJoin
            , miterLimit: miterLimit);
        var deflate = thickness * 0.5;
        var p1 = new Point(x: state.XAxisCursorPosition + deflate, y: 0);
        var p2 = new Point(x: state.XAxisCursorPosition + deflate, y: state.AreaHeight);
        var transform = context.PushPreTransform(
            Matrix.CreateTranslation(
                xPosition: state.AreaMargin.Left,
                yPosition: state.AreaMargin.Top));
        context.DrawLine(pen: pen, p1: p1, p2: p2);
        transform.Dispose();
    }

    private void DrawXAxis(
        DrawingContext context
        , LineChartState state
    )
    {
        var brush = XAxisStroke;
        if (brush is null
            || state.AreaWidth <= 0
            || state.AreaHeight <= 0
            || state.AreaWidth < XAxisMinViableWidth
            || state.AreaHeight < XAxisMinViableHeight)
        {
            return;
        }

        var size = XAxisArrowSize;
        var opacity = XAxisOpacity;
        var thickness = XAxisStrokeThickness;
        var pen = new Pen(brush: brush, thickness: thickness, dashStyle: null, lineCap: PenLineCap.Round);
        var deflate = thickness * 0.5;
        var offset = XAxisOffset;
        var p1 = new Point(
            x: state.AreaMargin.Left + offset.X,
            y: state.AreaMargin.Top + state.AreaHeight + offset.Y + deflate);
        var p2 = new Point(
            x: state.AreaMargin.Left + state.AreaWidth,
            y: state.AreaMargin.Top + state.AreaHeight + offset.Y + deflate);
        var opacityState = context.PushOpacity(opacity);
        context.DrawLine(pen: pen, p1: p1, p2: p2);
        var p3 = new Point(x: p2.X, y: p2.Y);
        var p4 = new Point(x: p2.X - size, y: p2.Y - size);
        context.DrawLine(pen: pen, p1: p3, p2: p4);
        var p5 = new Point(x: p2.X, y: p2.Y);
        var p6 = new Point(x: p2.X - size, y: p2.Y + size);
        context.DrawLine(pen: pen, p1: p5, p2: p6);
        opacityState.Dispose();
    }

    private static Point AlignXAxisLabelOffset(
        Point offsetCenter
        , double width
        , int index
        , int count
        , LabelAlignment alignment
    )
    {
        var isFirst = index == 0;
        var isLast = index == count - 1;

        return alignment switch
        {
            LabelAlignment.Auto => isFirst
                ? offsetCenter.WithX(offsetCenter.X - width / 2)
                : isLast
                    ? offsetCenter.WithX(offsetCenter.X - width - width / 2)
                    : offsetCenter.WithX(offsetCenter.X - width)
            , LabelAlignment.Left => offsetCenter.WithX(offsetCenter.X - width - width / 2)
            , LabelAlignment.Center => offsetCenter.WithX(offsetCenter.X - width)
            , LabelAlignment.Right => offsetCenter.WithX(offsetCenter.X - width / 2)
            , _ => offsetCenter.WithX(offsetCenter.X - width)
        };
    }

    private void DrawXAxisLabels(
        DrawingContext context
        , LineChartState state
    )
    {
        var foreground = XAxisLabelForeground;
        if (foreground is null
            || state.XAxisLabels is null
            || double.IsNaN(state.XAxisLabelStep)
            || state.ChartWidth <= 0
            || state.ChartHeight <= 0
            || state.ChartHeight - state.AreaMargin.Top < state.AreaMargin.Bottom)
        {
            return;
        }

        var opacity = XAxisLabelOpacity;
        var fontFamily = XAxisLabelFontFamily;
        var fontStyle = XAxisLabelFontStyle;
        var fontWeight = XAxisLabelFontWeight;
        var typeface = new Typeface(fontFamily: fontFamily, style: fontStyle, weight: fontWeight);
        var fontSize = XAxisLabelFontSize;
        var offset = XAxisLabelOffset;
        var angleRadians = Math.PI / 180.0 * XAxisLabelAngle;
        var alignment = XAxisLabelAlignment;
        var originTop = state.AreaMargin.Top + state.AreaHeight;
        var formattedTextLabels = new List<FormattedText>();
        var constrainWidthMax = 0.0;
        var constrainHeightMax = 0.0;
        var labels = state.XAxisLabels;

        for (var i = 0; i < labels.Count; i++)
        {
            var label = labels[i];
            var formattedText = CreateFormattedText(text: label, typeface: typeface /*, alignment: TextAlignment.Left*/
                , fontSize: fontSize /*, constraint: Size.Empty*/);
            formattedTextLabels.Add(formattedText);
            constrainWidthMax = Math.Max(val1: constrainWidthMax, val2: formattedText.Width);
            constrainHeightMax = Math.Max(val1: constrainHeightMax, val2: formattedText.Height);
        }

        var constraintMax = new Size(width: constrainWidthMax, height: constrainHeightMax);
        var offsetTransform =
            context.PushPreTransform(Matrix.CreateTranslation(xPosition: offset.X, yPosition: offset.Y));

        for (var i = 0; i < formattedTextLabels.Count; i++)
        {
            /*formattedTextLabels[i].Constraint = constraintMax;*/
            var origin = new Point(x: i * state.XAxisLabelStep + constraintMax.Width / 2 + state.AreaMargin.Left,
                y: originTop);
            var offsetCenter = new Point(x: constraintMax.Width / 2 - constraintMax.Width / 2, y: 0);
            offsetCenter = AlignXAxisLabelOffset(offsetCenter: offsetCenter, width: formattedTextLabels[i].Width
                , index: i, count: formattedTextLabels.Count, alignment: alignment);
            var xPosition = origin.X + constraintMax.Width / 2;
            var yPosition = origin.Y + constraintMax.Height / 2;
            var matrix = Matrix.CreateTranslation(xPosition: -xPosition, yPosition: -yPosition)
                         * Matrix.CreateRotation(angleRadians)
                         * Matrix.CreateTranslation(xPosition: xPosition, yPosition: yPosition);
            var labelTransform = context.PushPreTransform(matrix);
            var opacityState = context.PushOpacity(opacity);
            context.DrawText(text: formattedTextLabels[i], origin: origin + offsetCenter);
            opacityState.Dispose();
            labelTransform.Dispose();
        }

        offsetTransform.Dispose();
    }

    private void DrawXAxisTitle(
        DrawingContext context
        , LineChartState state
    )
    {
        var foreground = XAxisTitleForeground;
        if (foreground is null)
        {
            return;
        }

        if (state.AreaWidth <= 0
            || state.AreaHeight <= 0
            || state.AreaWidth < XAxisMinViableWidth
            || state.AreaHeight < XAxisMinViableHeight)
        {
            return;
        }

        var opacity = XAxisTitleOpacity;
        var fontFamily = XAxisTitleFontFamily;
        var fontStyle = XAxisTitleFontStyle;
        var fontWeight = XAxisTitleFontWeight;
        var typeface = new Typeface(fontFamily: fontFamily, style: fontStyle, weight: fontWeight);
        var fontSize = XAxisTitleFontSize;
        var offset = XAxisTitleOffset;
        var size = new Size(width: state.AreaWidth, height: XAxisTitleSize.Height);
        var angleRadians = Math.PI / 180.0 * XAxisTitleAngle;
        var alignment = XAxisTitleAlignment;
        var offsetTransform =
            context.PushPreTransform(Matrix.CreateTranslation(xPosition: offset.X, yPosition: offset.Y));
        var origin = new Point(x: state.AreaMargin.Left, y: state.AreaHeight + state.AreaMargin.Bottom);
        var constraint = new Size(width: size.Width, height: size.Height);
        var formattedText = CreateFormattedText(text: XAxisTitle, typeface: typeface /*, alignment: alignment*/
            , fontSize: fontSize /*, constraint: constraint*/);
        var xPosition = origin.X + size.Width / 2;
        var yPosition = origin.Y + size.Height / 2;

        var matrix = Matrix.CreateTranslation(xPosition: -xPosition, yPosition: -yPosition)
                     * Matrix.CreateRotation(angleRadians)
                     * Matrix.CreateTranslation(xPosition: xPosition, yPosition: yPosition);
        var labelTransform = context.PushPreTransform(matrix);
        var offsetCenter = new Point(x: 0, y: 0);
        var opacityState = context.PushOpacity(opacity);
        context.DrawText(text: formattedText, origin: origin + offsetCenter);
        opacityState.Dispose();
        labelTransform.Dispose();
        offsetTransform.Dispose();
    }

    private void DrawYAxis(
        DrawingContext context
        , LineChartState state
    )
    {
        var brush = YAxisStroke;
        if (brush is null
            || state.AreaWidth <= 0
            || state.AreaHeight <= 0
            || state.AreaWidth < YAxisMinViableWidth
            || state.AreaHeight < YAxisMinViableHeight)
        {
            return;
        }

        var size = YAxisArrowSize;
        var opacity = YAxisOpacity;
        var thickness = YAxisStrokeThickness;
        var pen = new Pen(brush: brush, thickness: thickness, dashStyle: null, lineCap: PenLineCap.Round);
        var deflate = thickness * 0.5;
        var offset = YAxisOffset;
        var p1 = new Point(
            x: state.AreaMargin.Left + offset.X + deflate,
            y: state.AreaMargin.Top);
        var p2 = new Point(
            x: state.AreaMargin.Left + offset.X + deflate,
            y: state.AreaMargin.Top + state.AreaHeight + offset.Y);
        var opacityState = context.PushOpacity(opacity);
        context.DrawLine(pen: pen, p1: p1, p2: p2);
        var p3 = new Point(x: p1.X, y: p1.Y);
        var p4 = new Point(x: p1.X - size, y: p1.Y + size);
        context.DrawLine(pen: pen, p1: p3, p2: p4);
        var p5 = new Point(x: p1.X, y: p1.Y);
        var p6 = new Point(x: p1.X + size, y: p1.Y + size);
        context.DrawLine(pen: pen, p1: p5, p2: p6);
        opacityState.Dispose();
    }

    private static TextAlignment GetYAxisLabelTextAlignment(
        LabelAlignment alignment
    ) =>
        alignment switch
        {
            LabelAlignment.Auto => TextAlignment.Right, LabelAlignment.Left => TextAlignment.Left
            , LabelAlignment.Center => TextAlignment.Center, LabelAlignment.Right => TextAlignment.Right
            , _ => TextAlignment.Center
        };

    private static Point AlignYAxisLabelOffset(
        Point offsetCenter
        , double height
        , int index
        , int count
        , LabelAlignment alignment
    )
    {
        var isFirst = index == 0;
        var isLast = index == count - 1;

        return alignment switch
        {
            LabelAlignment.Auto => isFirst
                ? offsetCenter.WithY(offsetCenter.Y + height / 2)
                : isLast
                    ? offsetCenter.WithY(offsetCenter.Y - height + height / 2)
                    : offsetCenter.WithY(offsetCenter.Y)
            , LabelAlignment.Left => offsetCenter, LabelAlignment.Center => offsetCenter
            , LabelAlignment.Right => offsetCenter, _ => offsetCenter
        };
    }

    private void DrawYAxisLabels(
        DrawingContext context
        , LineChartState state
    )
    {
        var foreground = YAxisLabelForeground;
        if (foreground is null
            || state.YAxisLabels is null
            || double.IsNaN(state.YAxisLabelStep)
            || state.ChartWidth <= 0
            || state.ChartWidth - state.AreaMargin.Right < state.AreaMargin.Left
            || state.ChartHeight <= 0)
        {
            return;
        }

        var opacity = YAxisLabelOpacity;
        var fontFamily = YAxisLabelFontFamily;
        var fontStyle = YAxisLabelFontStyle;
        var fontWeight = YAxisLabelFontWeight;
        var typeface = new Typeface(fontFamily: fontFamily, style: fontStyle, weight: fontWeight);
        var fontSize = YAxisLabelFontSize;
        var offset = YAxisLabelOffset;
        var angleRadians = Math.PI / 180.0 * YAxisLabelAngle;
        var alignment = YAxisLabelAlignment;
        var originLeft = state.AreaMargin.Left;
        var formattedTextLabels = new List<FormattedText>();
        var constrainWidthMax = 0.0;
        var constrainHeightMax = 0.0;
        var labels = state.YAxisLabels;

        for (var i = labels.Count - 1; i >= 0; i--)
        {
            var label = labels[i];
            var textAlignment = GetYAxisLabelTextAlignment(alignment);
            var formattedText = CreateFormattedText(text: label, typeface: typeface /*, alignment: textAlignment*/
                , fontSize: fontSize /*, constraint: Size.Empty*/);
            formattedTextLabels.Add(formattedText);
            constrainWidthMax = Math.Max(val1: constrainWidthMax, val2: formattedText.Width);
            constrainHeightMax = Math.Max(val1: constrainHeightMax, val2: formattedText.Height);
        }

        var constraintMax = new Size(width: constrainWidthMax, height: constrainHeightMax);
        var offsetTransform =
            context.PushPreTransform(Matrix.CreateTranslation(xPosition: offset.X, yPosition: offset.Y));

        for (var i = 0; i < formattedTextLabels.Count; i++)
        {
            /*formattedTextLabels[i].Constraint = constraintMax;*/

            var origin = new Point(x: originLeft,
                y: i * state.YAxisLabelStep - constraintMax.Height / 2 + state.AreaMargin.Top);
            var offsetCenter = new Point(x: constraintMax.Width / 2 - constraintMax.Width / 2, y: 0);
            offsetCenter = AlignYAxisLabelOffset(offsetCenter: offsetCenter
                , height: formattedTextLabels[i].Height, index: i, count: formattedTextLabels.Count
                , alignment: alignment);
            var xPosition = origin.X + constraintMax.Width / 2;
            var yPosition = origin.Y + constraintMax.Height / 2;
            var matrix = Matrix.CreateTranslation(xPosition: -xPosition, yPosition: -yPosition)
                         * Matrix.CreateRotation(angleRadians)
                         * Matrix.CreateTranslation(xPosition: xPosition, yPosition: yPosition);
            var labelTransform = context.PushPreTransform(matrix);
            var opacityState = context.PushOpacity(opacity);
            context.DrawText(text: formattedTextLabels[i], origin: origin + offsetCenter);
            opacityState.Dispose();
            labelTransform.Dispose();
        }

        offsetTransform.Dispose();
    }

    private void DrawYAxisTitle(
        DrawingContext context
        , LineChartState state
    )
    {
        var foreground = YAxisTitleForeground;
        if (foreground is null)
        {
            return;
        }

        if (state.AreaWidth <= 0
            || state.AreaHeight <= 0
            || state.AreaWidth < YAxisMinViableWidth
            || state.AreaHeight < YAxisMinViableHeight)
        {
            return;
        }

        var opacity = YAxisTitleOpacity;
        var fontFamily = YAxisTitleFontFamily;
        var fontStyle = YAxisTitleFontStyle;
        var fontWeight = YAxisTitleFontWeight;
        var typeface = new Typeface(fontFamily: fontFamily, style: fontStyle, weight: fontWeight);
        var fontSize = YAxisTitleFontSize;
        var offset = YAxisTitleOffset;
        var size = YAxisTitleSize;
        var angleRadians = Math.PI / 180.0 * YAxisTitleAngle;
        var alignment = YAxisTitleAlignment;
        var offsetTransform =
            context.PushPreTransform(Matrix.CreateTranslation(xPosition: offset.X, yPosition: offset.Y));
        var origin = new Point(x: state.AreaMargin.Left, y: state.AreaHeight + state.AreaMargin.Top);
        var constraint = new Size(width: size.Width, height: size.Height);
        var formattedText = CreateFormattedText(text: YAxisTitle, typeface: typeface /*, alignment: alignment*/
            , fontSize: fontSize /*, constraint: constraint*/);
        var xPosition = origin.X + size.Width / 2;
        var yPosition = origin.Y + size.Height / 2;
        var matrix = Matrix.CreateTranslation(xPosition: -xPosition, yPosition: -yPosition)
                     * Matrix.CreateRotation(angleRadians)
                     * Matrix.CreateTranslation(xPosition: xPosition, yPosition: yPosition);
        var labelTransform = context.PushPreTransform(matrix);
        var offsetCenter = new Point(x: 0, y: size.Height / 2 - formattedText.Height / 2);
        var opacityState = context.PushOpacity(opacity);
        context.DrawText(text: formattedText, origin: origin + offsetCenter);
        opacityState.Dispose();
        labelTransform.Dispose();
        offsetTransform.Dispose();
    }

    private void DrawBorder(
        DrawingContext context
        , LineChartState state
    )
    {
        var brush = BorderBrush;
        if (brush is null || state.AreaWidth <= 0 || state.AreaHeight <= 0)
        {
            return;
        }

        var thickness = BorderThickness;
        var radiusX = BorderRadiusX;
        var radiusY = BorderRadiusY;
        var pen = new Pen(brush: brush, thickness: thickness, dashStyle: null, lineCap: PenLineCap.Round);
        var rect = new Rect(x: 0, y: 0, width: state.ChartWidth, height: state.ChartHeight);
        var rectDeflate = rect.Deflate(thickness * 0.5);
        context.DrawRectangle(brush: Brushes.Transparent, pen: pen, rect: rectDeflate, radiusX: radiusX
            , radiusY: radiusY);
    }

    private void UpdateSubscription(
        INotifyCollectionChanged? oldValue
        , INotifyCollectionChanged? newValue
    )
    {
        if (oldValue is not null && _collectionChangedSubscriptions.TryGetValue(key: oldValue, value: out var value))
        {
            value.Dispose();
            _collectionChangedSubscriptions.Remove(oldValue);
        }

        if (newValue is not null)
        {
            newValue.CollectionChanged += ItemsPropertyCollectionChanged;

            _collectionChangedSubscriptions[newValue] =
                Disposable.Create(() => newValue.CollectionChanged -= ItemsPropertyCollectionChanged);
        }
    }

    private void ItemsPropertyCollectionChanged(
        object? sender
        , NotifyCollectionChangedEventArgs e
    ) => InvalidateVisual();

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change
    )
    {
        base.OnPropertyChanged(change);

        if (change.Property == XAxisValuesProperty || change.Property == YAxisValuesProperty ||
            change.Property == XAxisLabelsProperty || change.Property == YAxisLabelsProperty)
        {
            var (oldValue, newValue) = change.GetOldAndNewValue<INotifyCollectionChanged>();

            UpdateSubscription(
                oldValue: oldValue,
                newValue: newValue);
        }
    }

    public override void Render(
        DrawingContext context
    )
    {
        base.Render(context);

        var state = CreateChartState(width: Bounds.Width, height: Bounds.Height);

        DrawAreaFill(context: context, state: state);
        DrawAreaStroke(context: context, state: state);
        DrawCursor(context: context, state: state);

        DrawXAxis(context: context, state: state);
        DrawXAxisTitle(context: context, state: state);
        DrawXAxisLabels(context: context, state: state);

        DrawYAxis(context: context, state: state);
        DrawYAxisTitle(context: context, state: state);
        DrawYAxisLabels(context: context, state: state);

        DrawBorder(context: context, state: state);
    }

    private class LineChartState
    {
        public double ChartWidth
        {
            get;
            set;
        }

        public double ChartHeight
        {
            get;
            set;
        }

        public double AreaWidth
        {
            get;
            set;
        }

        public double AreaHeight
        {
            get;
            set;
        }

        public Thickness AreaMargin
        {
            get;
            set;
        }

        public Point[]? Points
        {
            get;
            set;
        }

        public List<string>? XAxisLabels
        {
            get;
            set;
        }

        public double XAxisLabelStep
        {
            get;
            set;
        }

        public List<string>? YAxisLabels
        {
            get;
            set;
        }

        public double YAxisLabelStep
        {
            get;
            set;
        }

        public double XAxisCursorPosition
        {
            get;
            set;
        }
    }
}