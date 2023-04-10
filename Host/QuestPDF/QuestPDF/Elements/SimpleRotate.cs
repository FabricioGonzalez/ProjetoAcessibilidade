using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class SimpleRotate
        : ContainerElement
            , ICacheable
    {
        public int TurnCount
        {
            get;
            set;
        }

        public int NormalizedTurnCount => (TurnCount % 4 + 4) % 4;

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (NormalizedTurnCount == 0 || NormalizedTurnCount == 2)
            {
                return base.Measure(availableSpace: availableSpace);
            }

            availableSpace = new Size(width: availableSpace.Height, height: availableSpace.Width);
            var childSpace = base.Measure(availableSpace: availableSpace);

            if (childSpace.Type == SpacePlanType.Wrap)
            {
                return SpacePlan.Wrap();
            }

            var targetSpace = new Size(width: childSpace.Height, height: childSpace.Width);

            if (childSpace.Type == SpacePlanType.FullRender)
            {
                return SpacePlan.FullRender(size: targetSpace);
            }

            if (childSpace.Type == SpacePlanType.PartialRender)
            {
                return SpacePlan.PartialRender(size: targetSpace);
            }

            throw new ArgumentException();
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var translate = new Position(
                x: NormalizedTurnCount == 1 || NormalizedTurnCount == 2 ? availableSpace.Width : 0,
                y: NormalizedTurnCount == 2 || NormalizedTurnCount == 3 ? availableSpace.Height : 0);

            var rotate = NormalizedTurnCount * 90;

            Canvas.Translate(vector: translate);
            Canvas.Rotate(angle: rotate);

            if (NormalizedTurnCount == 1 || NormalizedTurnCount == 3)
            {
                availableSpace = new Size(width: availableSpace.Height, height: availableSpace.Width);
            }

            Child?.Draw(availableSpace: availableSpace);

            Canvas.Rotate(angle: -rotate);
            Canvas.Translate(vector: translate.Reverse());
        }
    }
}