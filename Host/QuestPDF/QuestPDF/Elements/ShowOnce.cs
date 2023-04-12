using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class ShowOnce
        : ContainerElement
            , IStateResettable
            , ICacheable
    {
        private bool IsRendered
        {
            get;
            set;
        }

        public void ResetState() => IsRendered = false;

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (Child == null || IsRendered)
            {
                return SpacePlan.FullRender(width: 0, height: 0);
            }

            return base.Measure(availableSpace: availableSpace);
        }

        public override void Draw(
            Size availableSpace
        )
        {
            if (Child == null || IsRendered)
            {
                return;
            }

            if (base.Measure(availableSpace: availableSpace).Type == SpacePlanType.FullRender)
            {
                IsRendered = true;
            }

            base.Draw(availableSpace: availableSpace);
        }
    }
}