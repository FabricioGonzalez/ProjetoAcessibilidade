using System;

using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Padding : ContainerElement, ICacheable
    {
        public float Top
        {
            get; set;
        }
        public float Right
        {
            get; set;
        }
        public float Bottom
        {
            get; set;
        }
        public float Left
        {
            get; set;
        }

        public override SpacePlan Measure(Size availableSpace)
        {
            if (Child == null)
                return SpacePlan.FullRender(0, 0);

            var publicSpace = this.publicSpace(availableSpace);

            if (publicSpace.Width < 0 || publicSpace.Height < 0)
                return SpacePlan.Wrap();

            var measure = base.Measure(publicSpace);

            if (measure.Type == SpacePlanType.Wrap)
                return SpacePlan.Wrap();

            var newSize = new Size(
                measure.Width + Left + Right,
                measure.Height + Top + Bottom);

            if (measure.Type == SpacePlanType.PartialRender)
                return SpacePlan.PartialRender(newSize);

            if (measure.Type == SpacePlanType.FullRender)
                return SpacePlan.FullRender(newSize);

            throw new NotSupportedException();
        }

        public override void Draw(Size availableSpace)
        {
            if (Child == null)
                return;

            var publicSpace = this.publicSpace(availableSpace);

            Canvas.Translate(new Position(Left, Top));
            base.Draw(publicSpace);
            Canvas.Translate(new Position(-Left, -Top));
        }

        private Size publicSpace(Size availableSpace)
        {
            return new Size(
                availableSpace.Width - Left - Right,
                availableSpace.Height - Top - Bottom);
        }

        public override string ToString()
        {
            return $"Padding: Top({Top}) Right({Right}) Bottom({Bottom}) Left({Left})";
        }
    }
}