using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class ScaleToFit : ContainerElement
    {
        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (Child == null)
            {
                return SpacePlan.FullRender(size: Size.Zero);
            }

            var perfectScale = FindPerfectScale(child: Child, availableSpace: availableSpace);

            if (perfectScale == null)
            {
                return SpacePlan.Wrap();
            }

            var scaledSpace = ScaleSize(size: availableSpace, factor: 1 / perfectScale.Value);
            var childSizeInScale = Child.Measure(availableSpace: scaledSpace);
            var childSizeInOriginalScale = ScaleSize(size: childSizeInScale, factor: perfectScale.Value);
            return SpacePlan.FullRender(size: childSizeInOriginalScale);
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var perfectScale = FindPerfectScale(child: Child, availableSpace: availableSpace);

            if (!perfectScale.HasValue)
            {
                return;
            }

            var targetScale = perfectScale.Value;
            var targetSpace = ScaleSize(size: availableSpace, factor: 1 / targetScale);

            Canvas.Scale(scaleX: targetScale, scaleY: targetScale);
            Child?.Draw(availableSpace: targetSpace);
            Canvas.Scale(scaleX: 1 / targetScale, scaleY: 1 / targetScale);
        }

        private static Size ScaleSize(
            Size size
            , float factor
        ) => new(width: size.Width * factor, height: size.Height * factor);

        private static float? FindPerfectScale(
            Element child
            , Size availableSpace
        )
        {
            if (ChildFits(scale: 1))
            {
                return 1;
            }

            var maxScale = 1f;
            var minScale = Size.Epsilon;

            var lastWorkingScale = (float?)null;

            foreach (var _ in Enumerable.Range(start: 0, count: 8))
            {
                var halfScale = (maxScale + minScale) / 2;

                if (ChildFits(scale: halfScale))
                {
                    minScale = halfScale;
                    lastWorkingScale = halfScale;
                }
                else
                {
                    maxScale = halfScale;
                }
            }

            return lastWorkingScale;

            bool ChildFits(
                float scale
            )
            {
                var scaledSpace = ScaleSize(size: availableSpace, factor: 1 / scale);
                return child.Measure(availableSpace: scaledSpace).Type == SpacePlanType.FullRender;
            }
        }
    }
}