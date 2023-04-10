using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Constrained
        : ContainerElement
            , ICacheable
    {
        public float? MinWidth
        {
            get;
            set;
        }

        public float? MaxWidth
        {
            get;
            set;
        }

        public float? MinHeight
        {
            get;
            set;
        }

        public float? MaxHeight
        {
            get;
            set;
        }

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (MinWidth > availableSpace.Width + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            if (MinHeight > availableSpace.Height + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            var available = new Size(
                width: Min(x: MaxWidth, y: availableSpace.Width),
                height: Min(x: MaxHeight, y: availableSpace.Height));

            var measurement = base.Measure(availableSpace: available);

            if (measurement.Type == SpacePlanType.Wrap)
            {
                return SpacePlan.Wrap();
            }

            var actualSize = new Size(
                width: Max(x: MinWidth, y: measurement.Width),
                height: Max(x: MinHeight, y: measurement.Height));

            if (measurement.Type == SpacePlanType.FullRender)
            {
                return SpacePlan.FullRender(size: actualSize);
            }

            if (measurement.Type == SpacePlanType.PartialRender)
            {
                return SpacePlan.PartialRender(size: actualSize);
            }

            throw new NotSupportedException();
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var available = new Size(
                width: Min(x: MaxWidth, y: availableSpace.Width),
                height: Min(x: MaxHeight, y: availableSpace.Height));

            Child?.Draw(availableSpace: available);
        }

        private static float Min(
            float? x
            , float y
        ) => x.HasValue ? Math.Min(val1: x.Value, val2: y) : y;

        private static float Max(
            float? x
            , float y
        ) => x.HasValue ? Math.Max(val1: x.Value, val2: y) : y;
    }
}