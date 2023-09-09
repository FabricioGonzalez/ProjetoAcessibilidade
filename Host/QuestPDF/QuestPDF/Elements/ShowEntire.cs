using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class ShowEntire
        : ContainerElement
            , ICacheable
    {
        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            var childMeasurement = base.Measure(availableSpace: availableSpace);

            if (childMeasurement.Type == SpacePlanType.FullRender)
            {
                return childMeasurement;
            }

            return SpacePlan.Wrap();
        }
    }
}