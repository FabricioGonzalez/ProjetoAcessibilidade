using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class SkipOnce : ContainerElement, IStateResettable
    {
        private bool FirstPageWasSkipped { get; set; }

        public void ResetState()
        {
            FirstPageWasSkipped = false;
        }

        public override SpacePlan Measure(Size availableSpace)
        {
            if (Child == null || !FirstPageWasSkipped)
                return SpacePlan.FullRender(Size.Zero);

            return Child.Measure(availableSpace);
        }

        public override void Draw(Size availableSpace)
        {
            if (Child == null)
                return;

            if (FirstPageWasSkipped)
                Child.Draw(availableSpace);

            FirstPageWasSkipped = true;
        }
    }
}