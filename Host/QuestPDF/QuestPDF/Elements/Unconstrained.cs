using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Unconstrained
        : ContainerElement
            , ICacheable
    {
        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            var childSize = base.Measure(availableSpace: Size.Max);

            if (childSize.Type == SpacePlanType.PartialRender)
            {
                return SpacePlan.PartialRender(width: 0, height: 0);
            }

            if (childSize.Type == SpacePlanType.FullRender)
            {
                return SpacePlan.FullRender(width: 0, height: 0);
            }

            return childSize;
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var measurement = base.Measure(availableSpace: Size.Max);

            if (measurement.Type == SpacePlanType.Wrap)
            {
                return;
            }

            base.Draw(availableSpace: measurement);
        }
    }
}