using System;
using System.Collections.Generic;
using QuestPDF.Drawing;
using QuestPDF.Elements.Text.Calculation;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Text.Items
{
    public class TextBlockSpan : ITextBlockItem
    {
        private readonly Dictionary<(int startIndex, float availableWidth), TextMeasurementResult?>
            MeasureCache = new();

        public string Text
        {
            get;
            set;
        }

        public TextStyle Style
        {
            get;
            set;
        } = new();

        public virtual TextMeasurementResult? Measure(
            TextMeasurementRequest request
        )
        {
            var cacheKey = (request.StartIndex, request.AvailableWidth);

            if (!MeasureCache.ContainsKey(key: cacheKey))
            {
                MeasureCache[key: cacheKey] = MeasureWithoutCache(request: request);
            }

            return MeasureCache[key: cacheKey];
        }

        public virtual void Draw(
            TextDrawingRequest request
        )
        {
            var fontMetrics = Style.ToFontMetrics();

            var text = Text.Substring(startIndex: request.StartIndex, length: request.EndIndex - request.StartIndex);

            request.Canvas.DrawRectangle(vector: new Position(x: 0, y: request.TotalAscent)
                , size: new Size(width: request.TextSize.Width, height: request.TextSize.Height)
                , color: Style.BackgroundColor);
            request.Canvas.DrawText(text: text, position: Position.Zero, style: Style);

            // draw underline
            if ((Style.HasUnderline ?? false) && fontMetrics.UnderlinePosition.HasValue)
            {
                DrawLine(offset: fontMetrics.UnderlinePosition.Value, thickness: fontMetrics.UnderlineThickness ?? 1);
            }

            // draw stroke
            if ((Style.HasStrikethrough ?? false) && fontMetrics.StrikeoutPosition.HasValue)
            {
                DrawLine(offset: fontMetrics.StrikeoutPosition.Value, thickness: fontMetrics.StrikeoutThickness ?? 1);
            }

            void DrawLine(
                float offset
                , float thickness
            )
            {
                request.Canvas.DrawRectangle(vector: new Position(x: 0, y: offset - thickness / 2f)
                    , size: new Size(width: request.TextSize.Width, height: thickness), color: Style.Color);
            }
        }

        public TextMeasurementResult? MeasureWithoutCache(
            TextMeasurementRequest request
        )
        {
            const char space = ' ';

            var paint = Style.ToPaint();
            var fontMetrics = Style.ToFontMetrics();

            var startIndex = request.StartIndex;

            // if the element is the first one within the line,
            // ignore leading spaces
            if (request.IsFirstElementInLine)
            {
                while (startIndex < Text.Length && Text[index: startIndex] == space)
                {
                    startIndex++;
                }
            }

            if (Text.Length == 0 || startIndex == Text.Length)
            {
                return new TextMeasurementResult
                {
                    Width = 0, LineHeight = Style.LineHeight ?? 1, Ascent = fontMetrics.Ascent
                    , Descent = fontMetrics.Descent
                };
            }

            // start breaking text from requested position
            var text = Text.AsSpan().Slice(start: startIndex);

            var textLength = (int)paint.BreakText(text: text, maxWidth: request.AvailableWidth + Size.Epsilon);

            if (textLength <= 0)
            {
                return null;
            }

            // break text only on spaces
            var wrappedTextLength = WrapText(text: text, textLength: textLength
                , isFirstElementInLine: request.IsFirstElementInLine);

            if (wrappedTextLength == null)
            {
                return null;
            }

            textLength = wrappedTextLength.Value;

            text = text.Slice(start: 0, length: textLength);

            var endIndex = startIndex + textLength;
            var nextIndex = endIndex;

            // when breaking text, omit spaces at the end of the line
            while (nextIndex < Text.Length && Text[index: nextIndex] == space)
            {
                nextIndex++;
            }

            // measure final text
            var width = paint.MeasureText(text: text);

            return new TextMeasurementResult
            {
                Width = width, Ascent = fontMetrics.Ascent, Descent = fontMetrics.Descent
                , LineHeight = Style.LineHeight ?? 1, StartIndex = startIndex, EndIndex = endIndex
                , NextIndex = nextIndex, TotalIndex = Text.Length
            };

            static int? WrapText(
                ReadOnlySpan<char> text
                , int textLength
                , bool isFirstElementInLine
            )
            {
                // textLength - length of the part of the text that fits in available width (creating a line)

                // entire text fits, no need to wrap
                if (textLength == text.Length)
                {
                    return textLength;
                }

                // current line ends at word, next character is space, perfect place to wrap
                if (text[index: textLength - 1] != space && text[index: textLength] == space)
                {
                    return textLength;
                }

                // find last space within the available text to wrap
                var lastSpaceIndex = text.Slice(start: 0, length: textLength).LastIndexOf(value: space);

                // text contains space that can be used to wrap
                if (lastSpaceIndex > 0)
                {
                    return lastSpaceIndex;
                }

                // there is no available space to wrap text
                // if the item is first within the line, perform safe mode and chop the word
                // otherwise, move the item into the next line
                return isFirstElementInLine ? textLength : null;
            }
        }
    }
}