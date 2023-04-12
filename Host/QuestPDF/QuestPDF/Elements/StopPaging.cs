using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class StopPaging : ContainerElement
    {
        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (Child == null)
            {
                return SpacePlan.FullRender(size: Size.Zero);
            }

            var measurement = Child.Measure(availableSpace: availableSpace);

            return measurement.Type switch
            {
                SpacePlanType.Wrap => SpacePlan.FullRender(size: Size.Zero)
                , SpacePlanType.PartialRender => SpacePlan.FullRender(size: measurement)
                , SpacePlanType.FullRender => measurement, _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}