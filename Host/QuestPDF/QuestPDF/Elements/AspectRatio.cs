using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class AspectRatio
        : ContainerElement
            , ICacheable
    {
        public float Ratio
        {
            get;
            set;
        } = 1;

        public AspectRatioOption Option
        {
            get;
            set;
        } = AspectRatioOption.FitWidth;

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (Child == null)
            {
                return SpacePlan.FullRender(width: 0, height: 0);
            }

            var targetSize = GetTargetSize(availableSpace: availableSpace);

            if (targetSize.Height > availableSpace.Height + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            if (targetSize.Width > availableSpace.Width + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            var childSize = base.Measure(availableSpace: targetSize);

            if (childSize.Type == SpacePlanType.Wrap)
            {
                return SpacePlan.Wrap();
            }

            if (childSize.Type == SpacePlanType.PartialRender)
            {
                return SpacePlan.PartialRender(size: targetSize);
            }

            if (childSize.Type == SpacePlanType.FullRender)
            {
                return SpacePlan.FullRender(size: targetSize);
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

            var size = GetTargetSize(availableSpace: availableSpace);
            base.Draw(availableSpace: size);
        }

        private Size GetTargetSize(
            Size availableSpace
        )
        {
            var spaceRatio = availableSpace.Width / availableSpace.Height;

            var fitHeight = new Size(width: availableSpace.Height * Ratio, height: availableSpace.Height);
            var fitWidth = new Size(width: availableSpace.Width, height: availableSpace.Width / Ratio);

            return Option switch
            {
                AspectRatioOption.FitWidth => fitWidth, AspectRatioOption.FitHeight => fitHeight
                , AspectRatioOption.FitArea => Ratio < spaceRatio ? fitHeight : fitWidth
                , _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}