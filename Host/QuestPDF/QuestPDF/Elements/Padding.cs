using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Padding
        : ContainerElement
            , ICacheable
    {
        public float Top
        {
            get;
            set;
        }

        public float Right
        {
            get;
            set;
        }

        public float Bottom
        {
            get;
            set;
        }

        public float Left
        {
            get;
            set;
        }

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (Child == null)
            {
                return SpacePlan.FullRender(width: 0, height: 0);
            }

            var publicSpace = this.publicSpace(availableSpace: availableSpace);

            if (publicSpace.Width < 0 || publicSpace.Height < 0)
            {
                return SpacePlan.Wrap();
            }

            var measure = base.Measure(availableSpace: publicSpace);

            if (measure.Type == SpacePlanType.Wrap)
            {
                return SpacePlan.Wrap();
            }

            var newSize = new Size(
                width: measure.Width + Left + Right,
                height: measure.Height + Top + Bottom);

            if (measure.Type == SpacePlanType.PartialRender)
            {
                return SpacePlan.PartialRender(size: newSize);
            }

            if (measure.Type == SpacePlanType.FullRender)
            {
                return SpacePlan.FullRender(size: newSize);
            }

            throw new NotSupportedException();
        }

        public override void Draw(
            Size availableSpace
        )
        {
            if (Child == null)
            {
                return;
            }

            var publicSpace = this.publicSpace(availableSpace: availableSpace);

            Canvas.Translate(vector: new Position(x: Left, y: Top));
            base.Draw(availableSpace: publicSpace);
            Canvas.Translate(vector: new Position(x: -Left, y: -Top));
        }

        private Size publicSpace(
            Size availableSpace
        ) =>
            new(
                width: availableSpace.Width - Left - Right,
                height: availableSpace.Height - Top - Bottom);

        public override string ToString() => $"Padding: Top({Top}) Right({Right}) Bottom({Bottom}) Left({Left})";
    }
}