﻿using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class StopPaging : ContainerElement
    {
        public override SpacePlan Measure(Size availableSpace)
        {
            if (Child == null)
                return SpacePlan.FullRender(Size.Zero);

            var measurement = Child.Measure(availableSpace);

            return measurement.Type switch
            {
                SpacePlanType.Wrap => SpacePlan.FullRender(Size.Zero),
                SpacePlanType.PartialRender => SpacePlan.FullRender(measurement),
                SpacePlanType.FullRender => measurement,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}