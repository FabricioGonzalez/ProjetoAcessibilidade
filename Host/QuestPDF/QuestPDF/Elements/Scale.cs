using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Scale
        : ContainerElement
            , ICacheable
    {
        public float ScaleX
        {
            get;
            set;
        } = 1;

        public float ScaleY
        {
            get;
            set;
        } = 1;

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            var targetSpace = new Size(
                width: Math.Abs(value: availableSpace.Width / ScaleX),
                height: Math.Abs(value: availableSpace.Height / ScaleY));

            var measure = base.Measure(availableSpace: targetSpace);

            if (measure.Type == SpacePlanType.Wrap)
            {
                return SpacePlan.Wrap();
            }

            var targetSize = new Size(
                width: Math.Abs(value: measure.Width * ScaleX),
                height: Math.Abs(value: measure.Height * ScaleY));

            if (measure.Type == SpacePlanType.PartialRender)
            {
                return SpacePlan.PartialRender(size: targetSize);
            }

            if (measure.Type == SpacePlanType.FullRender)
            {
                return SpacePlan.FullRender(size: targetSize);
            }

            throw new ArgumentException();
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var targetSpace = new Size(
                width: Math.Abs(value: availableSpace.Width / ScaleX),
                height: Math.Abs(value: availableSpace.Height / ScaleY));

            var translate = new Position(
                x: ScaleX < 0 ? availableSpace.Width : 0,
                y: ScaleY < 0 ? availableSpace.Height : 0);

            Canvas.Translate(vector: translate);
            Canvas.Scale(scaleX: ScaleX, scaleY: ScaleY);

            Child?.Draw(availableSpace: targetSpace);

            Canvas.Scale(scaleX: 1 / ScaleX, scaleY: 1 / ScaleY);
            Canvas.Translate(vector: translate.Reverse());
        }
    }
}