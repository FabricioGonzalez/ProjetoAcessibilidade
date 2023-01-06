﻿using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class PageBreak : Element, IStateResettable
    {
        private bool IsRendered { get; set; }
        
        public void ResetState()
        {
            IsRendered = false;
        }

        public override SpacePlan Measure(Size availableSpace)
        {
            if (IsRendered)
                return SpacePlan.FullRender(0, 0);

            return SpacePlan.PartialRender(Size.Zero);
        }

        public override void Draw(Size availableSpace)
        {
            IsRendered = true;
        }
    }
}