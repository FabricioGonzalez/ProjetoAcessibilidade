using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SkiaSharp;

namespace QuestPDF.Drawing
{
    public class FontStyleSet
    {
        private static readonly Dictionary<SKFontStyleSlant, List<SKFontStyleSlant>> SlantFallbacks = new()
        {
            {
                SKFontStyleSlant.Italic
                , new List<SKFontStyleSlant>
                    { SKFontStyleSlant.Italic, SKFontStyleSlant.Oblique, SKFontStyleSlant.Upright }
            }
            ,
            {
                SKFontStyleSlant.Oblique
                , new List<SKFontStyleSlant>
                    { SKFontStyleSlant.Oblique, SKFontStyleSlant.Italic, SKFontStyleSlant.Upright }
            }
            ,
 {
                SKFontStyleSlant.Upright
                , new List<SKFontStyleSlant>
                    { SKFontStyleSlant.Upright, SKFontStyleSlant.Oblique, SKFontStyleSlant.Italic }
            }
        };

        private ConcurrentDictionary<SKFontStyle, SKTypeface> Styles
        {
            get;
        } = new();

        public void Add(
            SKTypeface typeface
        )
        {
            var style = typeface.FontStyle;
            Styles.AddOrUpdate(key: style, addValueFactory: _ => typeface, updateValueFactory: (
                _
                , _
            ) => typeface);
        }

        public SKTypeface? Match(
            SKFontStyle target
        )
        {
            SKFontStyle? bestStyle = null;
            SKTypeface? bestTypeface = null;

            foreach (var entry in Styles)
            {
                if (IsBetterMatch(target: target, a: entry.Key, b: bestStyle))
                {
                    bestStyle = entry.Key;
                    bestTypeface = entry.Value;
                }
            }

            return bestTypeface;
        }

        // Checks whether style a is a better match for the target then style b. Uses the CSS font style matching algorithm
        public static bool IsBetterMatch(
            SKFontStyle? target
            , SKFontStyle? a
            , SKFontStyle? b
        )
        {
            // A font is better than no font
            if (b == null)
            {
                return true;
            }

            if (a == null)
            {
                return false;
            }

            // First check font width
            // For normal and condensed widths prefer smaller widths
            // For expanded widths prefer larger widths
            if (target.Width <= (int)SKFontStyleWidth.Normal)
            {
                if (a.Width <= target.Width && b.Width > target.Width)
                {
                    return true;
                }

                if (a.Width > target.Width && b.Width <= target.Width)
                {
                    return false;
                }
            }
            else
            {
                if (a.Width >= target.Width && b.Width < target.Width)
                {
                    return true;
                }

                if (a.Width < target.Width && b.Width >= target.Width)
                {
                    return false;
                }
            }

            // Prefer closest match
            var widthDifferenceA = Math.Abs(value: a.Width - target.Width);
            var widthDifferenceB = Math.Abs(value: b.Width - target.Width);

            if (widthDifferenceA < widthDifferenceB)
            {
                return true;
            }

            if (widthDifferenceB < widthDifferenceA)
            {
                return false;
            }

            // Prefer closest slant based on provided fallback list
            var slantFallback = SlantFallbacks[key: target.Slant];
            var slantIndexA = slantFallback.IndexOf(item: a.Slant);
            var slantIndexB = slantFallback.IndexOf(item: b.Slant);

            if (slantIndexA < slantIndexB)
            {
                return true;
            }

            if (slantIndexB < slantIndexA)
            {
                return false;
            }

            // Check weight last
            // For thin (<400) weights, prefer thinner weights
            // For regular (400-500) weights, prefer other regular weights, then use rule for thin or bold
            // For bold (>500) weights, prefer thicker weights
            // Behavior for values other than multiples of 100 is not given in the specification

            if (target.Weight >= 400 && target.Weight <= 500)
            {
                if (a.Weight >= 400 && a.Weight <= 500 && !(b.Weight >= 400 && b.Weight <= 500))
                {
                    return true;
                }

                if (!(a.Weight >= 400 && a.Weight <= 500) && b.Weight >= 400 && b.Weight <= 500)
                {
                    return false;
                }
            }

            if (target.Weight < 450)
            {
                if (a.Weight <= target.Weight && b.Weight > target.Weight)
                {
                    return true;
                }

                if (a.Weight > target.Weight && b.Weight <= target.Weight)
                {
                    return false;
                }
            }
            else
            {
                if (a.Weight >= target.Weight && b.Weight < target.Weight)
                {
                    return true;
                }

                if (a.Weight < target.Weight && b.Weight >= target.Weight)
                {
                    return false;
                }
            }

            // Prefer closest weight
            var weightDifferenceA = Math.Abs(value: a.Weight - target.Weight);
            var weightDifferenceB = Math.Abs(value: b.Weight - target.Weight);

            return weightDifferenceA < weightDifferenceB;
        }
    }
}