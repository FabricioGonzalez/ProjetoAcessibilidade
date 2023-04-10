using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class DecorationItemRenderingCommand
    {
        public Element Element
        {
            get;
            set;
        }

        public SpacePlan Measurement
        {
            get;
            set;
        }

        public Position Offset
        {
            get;
            set;
        }
    }

    public class Decoration
        : Element
            , ICacheable
    {
        public Element Before
        {
            get;
            set;
        } = new Empty();

        public Element Content
        {
            get;
            set;
        } = new Empty();

        public Element After
        {
            get;
            set;
        } = new Empty();

        public override IEnumerable<Element?> GetChildren()
        {
            yield return Before;
            yield return Content;
            yield return After;
        }

        public override void CreateProxy(
            Func<Element?, Element?> create
        )
        {
            Before = create(arg: Before);
            Content = create(arg: Content);
            After = create(arg: After);
        }

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            var renderingCommands = PlanLayout(availableSpace: availableSpace).ToList();

            if (renderingCommands.Any(predicate: x => x.Measurement.Type == SpacePlanType.Wrap))
            {
                return SpacePlan.Wrap();
            }

            var width = renderingCommands.Max(selector: x => x.Measurement.Width);
            var height = renderingCommands.Sum(selector: x => x.Measurement.Height);
            var size = new Size(width: width, height: height);

            if (width > availableSpace.Width + Size.Epsilon || height > availableSpace.Height + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            var willBeFullyRendered =
                renderingCommands.All(predicate: x => x.Measurement.Type == SpacePlanType.FullRender);

            return willBeFullyRendered
                ? SpacePlan.FullRender(size: size)
                : SpacePlan.PartialRender(size: size);
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var renderingCommands = PlanLayout(availableSpace: availableSpace).ToList();
            var width = renderingCommands.Max(selector: x => x.Measurement.Width);

            foreach (var command in renderingCommands)
            {
                var elementSize = new Size(width: width, height: command.Measurement.Height);

                Canvas.Translate(vector: command.Offset);
                command.Element.Draw(availableSpace: elementSize);
                Canvas.Translate(vector: command.Offset.Reverse());
            }
        }

        private IEnumerable<DecorationItemRenderingCommand> PlanLayout(
            Size availableSpace
        )
        {
            SpacePlan GetDecorationMeasurement(
                Element element
            )
            {
                var measurement = element.Measure(availableSpace: availableSpace);

                return measurement.Type == SpacePlanType.FullRender
                    ? measurement
                    : SpacePlan.Wrap();
            }

            var beforeMeasurement = GetDecorationMeasurement(element: Before);
            var afterMeasurement = GetDecorationMeasurement(element: After);

            var contentSpace = new Size(width: availableSpace.Width
                , height: availableSpace.Height - beforeMeasurement.Height - afterMeasurement.Height);
            var contentMeasurement = Content.Measure(availableSpace: contentSpace);

            yield return new DecorationItemRenderingCommand
            {
                Element = Before, Measurement = beforeMeasurement, Offset = Position.Zero
            };

            yield return new DecorationItemRenderingCommand
            {
                Element = Content, Measurement = contentMeasurement
                , Offset = new Position(x: 0, y: beforeMeasurement.Height)
            };

            yield return new DecorationItemRenderingCommand
            {
                Element = After, Measurement = afterMeasurement
                , Offset = new Position(x: 0, y: beforeMeasurement.Height + contentMeasurement.Height)
            };
        }
    }
}