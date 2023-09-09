using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class InlinedElement : Container
    {
    }

    public enum InlinedAlignment
    {
        Left
        , Center
        , Right
        , Justify
        , SpaceAround
    }

    public class Inlined
        : Element
            , IStateResettable
    {
        public List<InlinedElement> Elements
        {
            get;
            set;
        } = new();

        private Queue<InlinedElement> ChildrenQueue
        {
            get;
            set;
        }

        public float VerticalSpacing
        {
            get;
            set;
        }

        public float HorizontalSpacing
        {
            get;
            set;
        }

        public InlinedAlignment ElementsAlignment
        {
            get;
            set;
        }

        public VerticalAlignment BaselineAlignment
        {
            get;
            set;
        }

        public void ResetState() => ChildrenQueue = new Queue<InlinedElement>(collection: Elements);

        public override IEnumerable<Element?> GetChildren() => Elements;

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (!ChildrenQueue.Any())
            {
                return SpacePlan.FullRender(size: Size.Zero);
            }

            var lines = Compose(availableSize: availableSpace);

            if (!lines.Any())
            {
                return SpacePlan.Wrap();
            }

            var lineSizes = lines
                .Select(selector: line =>
                {
                    var size = GetLineSize(elements: line);

                    var widthWithSpacing = size.Width + (line.Count - 1) * HorizontalSpacing;
                    return new Size(width: widthWithSpacing, height: size.Height);
                })
                .ToList();

            var width = lineSizes.Max(selector: x => x.Width);
            var height = lineSizes.Sum(selector: x => x.Height) + (lines.Count - 1) * VerticalSpacing;
            var targetSize = new Size(width: width, height: height);

            var isPartiallyRendered = lines.Sum(selector: x => x.Count) != ChildrenQueue.Count;

            if (isPartiallyRendered)
            {
                return SpacePlan.PartialRender(size: targetSize);
            }

            return SpacePlan.FullRender(size: targetSize);
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var lines = Compose(availableSize: availableSpace);
            var topOffset = 0f;

            foreach (var line in lines)
            {
                var height = line
                    .Select(selector: x => x.Measure(availableSpace: Size.Max))
                    .Where(predicate: x => x.Type != SpacePlanType.Wrap)
                    .Max(selector: x => x.Height);

                DrawLine(elements: line);

                topOffset += height + VerticalSpacing;
                Canvas.Translate(vector: new Position(x: 0, y: height + VerticalSpacing));
            }

            Canvas.Translate(vector: new Position(x: 0, y: -topOffset));
            lines.SelectMany(selector: x => x).ToList().ForEach(action: x => ChildrenQueue.Dequeue());

            void DrawLine(
                ICollection<InlinedElement> elements
            )
            {
                var lineSize = GetLineSize(elements: elements);

                var elementOffset = ElementOffset();
                var leftOffset = AlignOffset();
                Canvas.Translate(vector: new Position(x: leftOffset, y: 0));

                foreach (var element in elements)
                {
                    var size = (Size)element.Measure(availableSpace: Size.Max);
                    var baselineOffset = BaselineOffset(elementSize: size, lineHeight: lineSize.Height);

                    if (size.Height == 0)
                    {
                        size = new Size(width: size.Width, height: lineSize.Height);
                    }

                    Canvas.Translate(vector: new Position(x: 0, y: baselineOffset));
                    element.Draw(availableSpace: size);
                    Canvas.Translate(vector: new Position(x: 0, y: -baselineOffset));

                    leftOffset += size.Width + elementOffset;
                    Canvas.Translate(vector: new Position(x: size.Width + elementOffset, y: 0));
                }

                Canvas.Translate(vector: new Position(x: -leftOffset, y: 0));

                float ElementOffset()
                {
                    var difference = availableSpace.Width - lineSize.Width;

                    if (elements.Count == 1)
                    {
                        return 0;
                    }

                    return ElementsAlignment switch
                    {
                        InlinedAlignment.Justify => difference / (elements.Count - 1)
                        , InlinedAlignment.SpaceAround => difference / (elements.Count + 1), _ => HorizontalSpacing
                    };
                }

                float AlignOffset()
                {
                    var difference = availableSpace.Width - lineSize.Width - (elements.Count - 1) * HorizontalSpacing;

                    return ElementsAlignment switch
                    {
                        InlinedAlignment.Left => 0, InlinedAlignment.Justify => 0
                        , InlinedAlignment.SpaceAround => elementOffset, InlinedAlignment.Center => difference / 2
                        , InlinedAlignment.Right => difference, _ => 0
                    };
                }

                float BaselineOffset(
                    Size elementSize
                    , float lineHeight
                )
                {
                    var difference = lineHeight - elementSize.Height;

                    return BaselineAlignment switch
                    {
                        VerticalAlignment.Top => 0, VerticalAlignment.Middle => difference / 2, _ => difference
                    };
                }
            }
        }

        private Size GetLineSize(
            ICollection<InlinedElement> elements
        )
        {
            var sizes = elements
                .Select(selector: x => x.Measure(availableSpace: Size.Max))
                .Where(predicate: x => x.Type != SpacePlanType.Wrap)
                .ToList();

            var width = sizes.Sum(selector: x => x.Width);
            var height = sizes.Max(selector: x => x.Height);

            return new Size(width: width, height: height);
        }

        // list of lines, each line is a list of elements
        private ICollection<ICollection<InlinedElement>> Compose(
            Size availableSize
        )
        {
            var queue = new Queue<InlinedElement>(collection: ChildrenQueue);
            var result = new List<ICollection<InlinedElement>>();

            var topOffset = 0f;

            while (true)
            {
                var line = GetNextLine();

                if (!line.Any())
                {
                    break;
                }

                var height = line
                    .Select(selector: x => x.Measure(availableSpace: availableSize))
                    .Where(predicate: x => x.Type != SpacePlanType.Wrap)
                    .Max(selector: x => x.Height);

                if (topOffset + height > availableSize.Height + Size.Epsilon)
                {
                    break;
                }

                topOffset += height + VerticalSpacing;
                result.Add(item: line);
            }

            return result;

            ICollection<InlinedElement> GetNextLine()
            {
                var result = new List<InlinedElement>();
                var leftOffset = GetInitialAlignmentOffset();

                while (true)
                {
                    if (!queue.Any())
                    {
                        break;
                    }

                    var element = queue.Peek();
                    var size = element.Measure(availableSpace: Size.Max);

                    if (size.Type == SpacePlanType.Wrap)
                    {
                        break;
                    }

                    if (leftOffset + size.Width > availableSize.Width + Size.Epsilon)
                    {
                        break;
                    }

                    queue.Dequeue();
                    leftOffset += size.Width + HorizontalSpacing;
                    result.Add(item: element);
                }

                return result;
            }

            float GetInitialAlignmentOffset()
            {
                // this method makes sure that the spacing between elements is no lesser than configured

                return ElementsAlignment switch
                {
                    InlinedAlignment.SpaceAround => HorizontalSpacing * 2, _ => 0
                };
            }
        }
    }
}