using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Elements.Text.Calculation;
using QuestPDF.Elements.Text.Items;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Text
{
    public class TextBlock
        : Element
            , IStateResettable
    {
        public HorizontalAlignment Alignment
        {
            get;
            set;
        } = HorizontalAlignment.Left;

        public List<ITextBlockItem> Items
        {
            get;
            set;
        } = new();

        public string Text => string.Join(separator: " "
            , values: Items.Where(predicate: x => x is TextBlockSpan).Cast<TextBlockSpan>()
                .Select(selector: x => x.Text));

        private Queue<ITextBlockItem> RenderingQueue
        {
            get;
            set;
        }

        private int CurrentElementIndex
        {
            get;
            set;
        }

        public void ResetState()
        {
            RenderingQueue = new Queue<ITextBlockItem>(collection: Items);
            CurrentElementIndex = 0;
        }

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (!RenderingQueue.Any())
            {
                return SpacePlan.FullRender(size: Size.Zero);
            }

            var lines = DivideTextItemsIntoLines(availableWidth: availableSpace.Width
                , availableHeight: availableSpace.Height).ToList();

            if (!lines.Any())
            {
                return SpacePlan.Wrap();
            }

            var width = lines.Max(selector: x => x.Width);
            var height = lines.Sum(selector: x => x.LineHeight);

            if (width > availableSpace.Width + Size.Epsilon || height > availableSpace.Height + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            var fullyRenderedItemsCount = lines
                .SelectMany(selector: x => x.Elements)
                .GroupBy(keySelector: x => x.Item)
                .Count(predicate: x => x.Any(predicate: y => y.Measurement.IsLast));

            if (fullyRenderedItemsCount == RenderingQueue.Count)
            {
                return SpacePlan.FullRender(width: width, height: height);
            }

            return SpacePlan.PartialRender(width: width, height: height);
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var lines = DivideTextItemsIntoLines(availableWidth: availableSpace.Width
                , availableHeight: availableSpace.Height).ToList();

            if (!lines.Any())
            {
                return;
            }

            var heightOffset = 0f;
            var widthOffset = 0f;

            foreach (var line in lines)
            {
                widthOffset = 0f;

                var alignmentOffset = GetAlignmentOffset(lineWidth: line.Width);

                Canvas.Translate(vector: new Position(x: alignmentOffset, y: 0));
                Canvas.Translate(vector: new Position(x: 0, y: -line.Ascent));

                foreach (var item in line.Elements)
                {
                    var textDrawingRequest = new TextDrawingRequest
                    {
                        Canvas = Canvas, PageContext = PageContext, StartIndex = item.Measurement.StartIndex
                        , EndIndex = item.Measurement.EndIndex
                        , TextSize = new Size(width: item.Measurement.Width, height: line.LineHeight)
                        , TotalAscent = line.Ascent
                    };

                    item.Item.Draw(request: textDrawingRequest);

                    Canvas.Translate(vector: new Position(x: item.Measurement.Width, y: 0));
                    widthOffset += item.Measurement.Width;
                }

                Canvas.Translate(vector: new Position(x: -alignmentOffset, y: 0));
                Canvas.Translate(vector: new Position(x: -line.Width, y: line.Ascent));
                Canvas.Translate(vector: new Position(x: 0, y: line.LineHeight));

                heightOffset += line.LineHeight;
            }

            Canvas.Translate(vector: new Position(x: 0, y: -heightOffset));

            lines
                .SelectMany(selector: x => x.Elements)
                .GroupBy(keySelector: x => x.Item)
                .Where(predicate: x => x.Any(predicate: y => y.Measurement.IsLast))
                .Select(selector: x => x.Key)
                .ToList()
                .ForEach(action: x => RenderingQueue.Dequeue());

            var lastElementMeasurement = lines.Last().Elements.Last().Measurement;
            CurrentElementIndex = lastElementMeasurement.IsLast ? 0 : lastElementMeasurement.NextIndex;

            if (!RenderingQueue.Any())
            {
                ResetState();
            }

            float GetAlignmentOffset(
                float lineWidth
            )
            {
                if (Alignment == HorizontalAlignment.Left)
                {
                    return 0;
                }

                var emptySpace = availableSpace.Width - lineWidth;

                if (Alignment == HorizontalAlignment.Right)
                {
                    return emptySpace;
                }

                if (Alignment == HorizontalAlignment.Center)
                {
                    return emptySpace / 2;
                }

                throw new ArgumentException();
            }
        }

        public IEnumerable<TextLine> DivideTextItemsIntoLines(
            float availableWidth
            , float availableHeight
        )
        {
            var queue = new Queue<ITextBlockItem>(collection: RenderingQueue);
            var currentItemIndex = CurrentElementIndex;
            var currentHeight = 0f;

            while (queue.Any())
            {
                var line = GetNextLine();

                if (!line.Elements.Any())
                {
                    yield break;
                }

                if (currentHeight + line.LineHeight > availableHeight + Size.Epsilon)
                {
                    yield break;
                }

                currentHeight += line.LineHeight;
                yield return line;
            }

            TextLine GetNextLine()
            {
                var currentWidth = 0f;

                var currentLineElements = new List<TextLineElement>();

                while (true)
                {
                    if (!queue.Any())
                    {
                        break;
                    }

                    var currentElement = queue.Peek();

                    var measurementRequest = new TextMeasurementRequest
                    {
                        Canvas = Canvas, PageContext = PageContext, StartIndex = currentItemIndex
                        , AvailableWidth = availableWidth - currentWidth
                        , IsFirstElementInLine = !currentLineElements.Any()
                    };

                    var measurementResponse = currentElement.Measure(request: measurementRequest);

                    if (measurementResponse == null)
                    {
                        break;
                    }

                    currentLineElements.Add(item: new TextLineElement
                    {
                        Item = currentElement, Measurement = measurementResponse
                    });

                    currentWidth += measurementResponse.Width;
                    currentItemIndex = measurementResponse.NextIndex;

                    if (!measurementResponse.IsLast)
                    {
                        break;
                    }

                    currentItemIndex = 0;
                    queue.Dequeue();
                }

                return TextLine.From(elements: currentLineElements);
            }
        }
    }
}