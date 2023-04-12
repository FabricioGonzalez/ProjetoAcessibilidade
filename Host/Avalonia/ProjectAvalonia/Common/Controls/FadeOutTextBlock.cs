using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Avalonia.Styling;

namespace ProjectAvalonia.Common.Controls;

public class FadeOutTextBlock
    : TextBlock
        , IStyleable
{
    private static readonly IBrush FadeoutOpacityMask = new LinearGradientBrush
    {
        StartPoint = new RelativePoint(x: 0, y: 0, unit: RelativeUnit.Relative)
        , EndPoint = new RelativePoint(x: 1, y: 0, unit: RelativeUnit.Relative), GradientStops =
        {
            new GradientStop { Color = Colors.White, Offset = 0 }
            , new GradientStop { Color = Colors.White, Offset = 0.7 }
            , new GradientStop { Color = Colors.Transparent, Offset = 0.9 }
        }
    }.ToImmutable();

    private Size _constraint;
    private bool _cutOff;
    private TextLayout? _noTrimLayout;
    private TextLayout? _trimmedLayout;

    public FadeOutTextBlock()
    {
        TextWrapping = TextWrapping.NoWrap;
    }

    public Type StyleKey
    {
        get;
    } = typeof(TextBlock);

    public override void Render(
        DrawingContext context
    )
    {
        var background = Background;

        var bounds = Bounds;

        if (background != null)
        {
            context.FillRectangle(brush: background, rect: Bounds);
        }

        if (_trimmedLayout is null || _noTrimLayout is null)
        {
            return;
        }

        var width = bounds.Size.Width;

        var centerOffset = TextAlignment switch
        {
            TextAlignment.Center => (width - _trimmedLayout.Size.Width) / 2.0
            , TextAlignment.Right => width - _trimmedLayout.Size.Width, _ => 0.0
        };

        var (left, yPosition, _, _) = Padding;

        using var a =
            context.PushPostTransform(matrix: Matrix.CreateTranslation(xPosition: left + centerOffset
                , yPosition: yPosition));
        using var b = _cutOff ? context.PushOpacityMask(mask: FadeoutOpacityMask, bounds: Bounds) : Disposable.Empty;
        _noTrimLayout.Draw(context: context);
    }

    private void NewCreateTextLayout(
        Size constraint
        , string? text
    )
    {
        if (constraint == Size.Empty)
        {
            _trimmedLayout = null;
        }

        var text1 = text ?? "";
        var typeface = new Typeface(fontFamily: FontFamily, style: FontStyle, weight: FontWeight);
        var fontSize = FontSize;
        var foreground = Foreground;
        var textAlignment = TextAlignment;
        var textWrapping = TextWrapping;
        var textDecorations = TextDecorations;
        var width = constraint.Width;
        var height = constraint.Height;
        var lineHeight = LineHeight;

        _noTrimLayout = new TextLayout(
            text: text1,
            typeface: typeface,
            fontSize: fontSize,
            foreground: foreground,
            textAlignment: textAlignment,
            textWrapping: textWrapping,
            textTrimming: TextTrimming.None,
            textDecorations: textDecorations,
            maxWidth: width,
            maxHeight: height,
            lineHeight: lineHeight,
            maxLines: 1);

        _trimmedLayout = new TextLayout(
            text: text1,
            typeface: typeface,
            fontSize: fontSize,
            foreground: foreground,
            textAlignment: textAlignment,
            textWrapping: textWrapping,
            textTrimming: TextTrimming.CharacterEllipsis,
            textDecorations: textDecorations,
            maxWidth: width,
            maxHeight: height,
            lineHeight: lineHeight,
            maxLines: 1);

        _cutOff = _trimmedLayout.TextLines[index: 0].HasCollapsed;
    }

    protected override Size MeasureOverride(
        Size availableSize
    )
    {
        if (string.IsNullOrEmpty(value: Text))
        {
            return new Size();
        }

        var padding = Padding;

        availableSize = availableSize.Deflate(thickness: padding);

        if (_constraint != availableSize)
        {
            _constraint = availableSize;
            NewCreateTextLayout(constraint: _constraint, text: Text);
        }

        return (_trimmedLayout?.Size ?? Size.Empty).Inflate(thickness: padding);
    }
}