/*using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Utilities;

namespace ProjectAvalonia.Common.Controls;

public class PrivacyTextPresenter : UserControl
{
    private FormattedText? _formattedText;
    private GlyphRun? _glyphRun;
    private double _width;

    private FormattedText CreateFormattedText() =>
        new(
            textToFormat: "",
            typeface: new Typeface(fontFamily: FontFamily, style: FontStyle, weight: FontWeight),
            emSize: FontSize,
            culture: CultureInfo.CurrentUICulture,
            flowDirection: FlowDirection.LeftToRight,
            foreground: null
            /*textAlignment: TextAlignment.Left,
            textWrapping: TextWrapping.NoWrap,#1#
            /*constraint: Size.Infinity#1#);

    private GlyphRun? CreateGlyphRun(
        double width
    )
    {
        var privacyChar = '#';

        var glyphTypeface = new Typeface(fontFamily: (FontFamily?)FontFamily);
        var glyph = glyphTypeface.GlyphTypeface.GetGlyph(codepoint: privacyChar);

        var scale = FontSize / glyphTypeface.GlyphTypeface.DesignEmHeight;
        var advance = glyphTypeface.GetGlyphAdvance(glyph: glyph) * scale;

        var count = width > 0 && width < advance ? 1 : (int)(width / advance);
        if (count == 0)
        {
            return null;
        }

        var advances = new ReadOnlySlice<double>(
            buffer: new ReadOnlyMemory<double>(array: Enumerable.Repeat(element: advance, count: count).ToArray()));
        var characters =
            new ReadOnlySlice<char>(
                buffer: new ReadOnlyMemory<char>(array: Enumerable.Repeat(element: privacyChar, count: count)
                    .ToArray()));
        var glyphs = new ReadOnlySlice<ushort>(
            buffer: new ReadOnlyMemory<ushort>(array: Enumerable.Repeat(element: glyph, count: count).ToArray()));

        return new GlyphRun(glyphTypeface: glyphTypeface, fontRenderingEmSize: FontSize, glyphIndices: glyphs
            , glyphAdvances: advances, characters: characters);
    }

    protected override Size MeasureOverride(
        Size availableSize
    )
    {
        _formattedText ??= CreateFormattedText();

        return new Size(width: 0, height: _formattedText.Bounds.Height);
    }

    public override void Render(
        DrawingContext context
    )
    {
        if (double.IsNaN(d: Bounds.Width) || Bounds.Width == 0)
        {
            return;
        }

        var width = Bounds.Width;
        if (_glyphRun is null || width != _width)
        {
            (_glyphRun as IDisposable)?.Dispose();
            _glyphRun = CreateGlyphRun(width: width);
            _width = width;
        }

        if (_glyphRun is not null)
        {
            context.DrawGlyphRun(foreground: Foreground, glyphRun: _glyphRun);
        }
    }
}*/

