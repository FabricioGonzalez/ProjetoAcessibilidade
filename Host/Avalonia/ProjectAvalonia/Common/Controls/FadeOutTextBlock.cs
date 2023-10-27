using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;

namespace ProjectAvalonia.Common.Controls;

public class FadeOutTextBlock
    : Control
{
    private static readonly IBrush FadeoutOpacityMask = new LinearGradientBrush
    {
        StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
        EndPoint = new RelativePoint(1, 0, RelativeUnit.Relative), GradientStops =
        {
            new GradientStop { Color = Colors.White, Offset = 0 },
            new GradientStop { Color = Colors.White, Offset = 0.7 },
            new GradientStop { Color = Colors.Transparent, Offset = 0.9 }
        }
    }.ToImmutable();

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<FadeOutTextBlock, string>(nameof(Text), defaultBindingMode: BindingMode.TwoWay);

    private Size _constraint;
    private bool _cutOff;
    private TextLayout? _noTrimLayout;
    private TextLayout? _trimmedLayout;


    public FadeOutTextBlock()
    {
        Wrapping = TextWrapping.NoWrap;
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public IBrush? Background
    {
        get;
        set;
    }

    public TextWrapping? Wrapping
    {
        get;
        set;
    }

    public TextAlignment? Alignment
    {
        get;
        set;
    }

    public Thickness Padding
    {
        get;
        set;
    }

    public Type StyleKey
    {
        get;
    } = typeof(TextBlock);

    public FontWeight FontWeight
    {
        get;
        set;
    } = FontWeight.Normal;

    public FontStyle FontStyle
    {
        get;
        set;
    } = FontStyle.Normal;

    public FontFamily FontFamily
    {
        get;
        set;
    } = FontFamily.Default;

    public double LineHeight
    {
        get;
        set;
    }

    public TextDecorationCollection? Decorations
    {
        get;
        set;
    }

    public IBrush Foreground
    {
        get;
        set;
    }

    public double FontSize
    {
        get;
        set;
    } = 12;

    public TextTrimming Trimming
    {
        get;
        set;
    }


    public override sealed void Render(
        DrawingContext context
    )
    {
        var background = Background;

        var bounds = Bounds;

        if (background != null)
        {
            context.FillRectangle(background, Bounds);
        }

        if (_trimmedLayout is null || _noTrimLayout is null)
        {
            return;
        }

        var width = bounds.Size.Width;

        var centerOffset = Alignment switch
        {
            TextAlignment.Center => (width - _trimmedLayout.Width) / 2.0,
            TextAlignment.Right => width - _trimmedLayout.Width, _ => 0.0
        };

        var (left, yPosition, _, _) = Padding;

        using var a =
            context.PushTransform(Matrix.CreateTranslation(left + centerOffset
                , yPosition));
        using var b = _cutOff ? context.PushOpacityMask(FadeoutOpacityMask, Bounds) : Disposable.Empty;

        if (!_cutOff)
        {
            _noTrimLayout.Draw(context, new Point(0, -10));
            return;
        }

        _trimmedLayout.Draw(context, new Point(0, 0));
    }

    private void NewCreateTextLayout(
        Size constraint
        , string? text
    )
    {
        if (constraint == new Size(0, 0))
        {
            _trimmedLayout = null;
        }

        var text1 = text ?? "";
        var typeface = new Typeface(FontFamily, FontStyle, FontWeight);
        var fontSize = FontSize;
        var foreground = Foreground;
        var textAlignment = Alignment ?? TextAlignment.Justify;
        var textWrapping = Wrapping ?? TextWrapping.NoWrap;
        var textDecorations = Decorations;
        var width = constraint.Width;
        var height = constraint.Height;
        var lineHeight = LineHeight;

        _noTrimLayout = new TextLayout(
            text1,
            typeface,
            fontSize,
            foreground,
            textAlignment,
            textWrapping,
            Trimming,
            textDecorations,
            FlowDirection.LeftToRight,
            width,
            height,
            lineHeight,
            1);

        _trimmedLayout = new TextLayout(
            text1,
            typeface,
            fontSize,
            foreground,
            textAlignment,
            textWrapping,
            Trimming,
            textDecorations,
            FlowDirection.LeftToRight,
            width,
            height,
            lineHeight);

        _cutOff = _trimmedLayout.TextLines[0].HasCollapsed || _trimmedLayout.TextLines.Count > 1;
    }

    protected override Size MeasureOverride(
        Size availableSize
    )
    {
        if (string.IsNullOrEmpty(Text))
        {
            return new Size();
        }

        var padding = Padding;

        availableSize = availableSize.Deflate(padding);

        if (_constraint != availableSize)
        {
            _constraint = availableSize;
            NewCreateTextLayout(_constraint, Text);
        }

        return new Size(_trimmedLayout?.Width ?? 0, _trimmedLayout?.Height ?? 0).Inflate(padding);
    }
}