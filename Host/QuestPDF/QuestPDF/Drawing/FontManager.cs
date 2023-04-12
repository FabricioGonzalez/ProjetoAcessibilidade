using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Drawing
{
    public static class FontManager
    {
        private static readonly ConcurrentDictionary<string, FontStyleSet> StyleSets = new();
        private static readonly ConcurrentDictionary<object, SKFontMetrics> FontMetrics = new();
        private static readonly ConcurrentDictionary<object, SKPaint> Paints = new();
        private static readonly ConcurrentDictionary<string, SKPaint> ColorPaint = new();

        private static void RegisterFontType(
            SKData fontData
            , string? customName = null
        )
        {
            foreach (var index in Enumerable.Range(start: 0, count: 256))
            {
                var typeface = SKTypeface.FromData(data: fontData, index: index);

                if (typeface == null)
                {
                    break;
                }

                var typefaceName = customName ?? typeface.FamilyName;

                var fontStyleSet = StyleSets.GetOrAdd(key: typefaceName, valueFactory: _ => new FontStyleSet());
                fontStyleSet.Add(typeface: typeface);
            }
        }

        [Obsolete(
            message:
            "Since version 2022.3, the FontManager class offers better font type matching support. Please use the RegisterFont(Stream stream) method.")]
        public static void RegisterFontType(
            string fontName
            , Stream stream
        )
        {
            using var fontData = SKData.Create(stream: stream);
            RegisterFontType(fontData: fontData);
            RegisterFontType(fontData: fontData, customName: fontName);
        }

        public static void RegisterFont(
            Stream stream
        )
        {
            using var fontData = SKData.Create(stream: stream);
            RegisterFontType(fontData: fontData);
        }

        public static SKPaint ColorToPaint(
            this string color
        )
        {
            return ColorPaint.GetOrAdd(key: color, valueFactory: Convert);

            static SKPaint Convert(
                string color
            )
            {
                return new SKPaint
                {
                    Color = SKColor.Parse(hexString: color), IsAntialias = true
                };
            }
        }

        public static SKPaint ToPaint(
            this TextStyle style
        )
        {
            return Paints.GetOrAdd(key: style.PaintKey, valueFactory: key => Convert(style: style));

            static SKPaint Convert(
                TextStyle style
            )
            {
                return new SKPaint
                {
                    Color = SKColor.Parse(hexString: style.Color), Typeface = GetTypeface(style: style)
                    , TextSize = style.Size ?? 12, IsAntialias = true
                };
            }

            static SKTypeface GetTypeface(
                TextStyle style
            )
            {
                var weight = (SKFontStyleWeight)(style.FontWeight ?? FontWeight.Normal);
                var slant = style.IsItalic ?? false ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

                var fontStyle = new SKFontStyle(weight: weight, width: SKFontStyleWidth.Normal, slant: slant);

                if (StyleSets.TryGetValue(key: style.FontFamily, value: out var fontStyleSet))
                {
                    return fontStyleSet.Match(target: fontStyle);
                }

                var fontFromDefaultSource =
                    SKFontManager.Default.MatchFamily(familyName: style.FontFamily, style: fontStyle);

                if (fontFromDefaultSource != null)
                {
                    return fontFromDefaultSource;
                }

                throw new ArgumentException(
                    message: $"The typeface '{style.FontFamily}' could not be found. " +
                             "Please consider the following options: " +
                             "1) install the font on your operating system or execution environment. " +
                             "2) load a font file specifically for QuestPDF usage via the QuestPDF.Drawing.FontManager.RegisterFontType(Stream fileContentStream) static method.");
            }
        }

        public static SKFontMetrics ToFontMetrics(
            this TextStyle style
        ) => FontMetrics.GetOrAdd(key: style.FontMetricsKey, valueFactory: key => style.ToPaint().FontMetrics);
    }
}