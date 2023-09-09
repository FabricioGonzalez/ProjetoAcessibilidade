using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Alignment : ContainerElement
    {
        public VerticalAlignment Vertical
        {
            get;
            set;
        } = VerticalAlignment.Top;

        public HorizontalAlignment Horizontal
        {
            get;
            set;
        } = HorizontalAlignment.Left;

        public override void Draw(
            Size availableSpace
        )
        {
            if (Child == null)
            {
                return;
            }

            var childSize = base.Measure(availableSpace: availableSpace);

            if (childSize.Type == SpacePlanType.Wrap)
            {
                return;
            }

            var top = GetTopOffset(availableSpace: availableSpace, childSize: childSize);
            var left = GetLeftOffset(availableSpace: availableSpace, childSize: childSize);

            Canvas.Translate(vector: new Position(x: left, y: top));
            base.Draw(availableSpace: childSize);
            Canvas.Translate(vector: new Position(x: -left, y: -top));
        }

        private float GetTopOffset(
            Size availableSpace
            , Size childSize
        )
        {
            var difference = availableSpace.Height - childSize.Height;

            return Vertical switch
            {
                VerticalAlignment.Top => 0, VerticalAlignment.Middle => difference / 2
                , VerticalAlignment.Bottom => difference, _ => throw new NotSupportedException()
            };
        }

        private float GetLeftOffset(
            Size availableSpace
            , Size childSize
        )
        {
            var difference = availableSpace.Width - childSize.Width;

            return Horizontal switch
            {
                HorizontalAlignment.Left => 0, HorizontalAlignment.Center => difference / 2
                , HorizontalAlignment.Right => difference, _ => throw new NotSupportedException()
            };
        }
    }
}