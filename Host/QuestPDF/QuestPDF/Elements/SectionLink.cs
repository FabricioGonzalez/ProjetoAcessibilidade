using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class SectionLink : ContainerElement
    {
        public string SectionName
        {
            get;
            set;
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var targetSize = base.Measure(availableSpace: availableSpace);

            if (targetSize.Type == SpacePlanType.Wrap)
            {
                return;
            }

            Canvas.DrawSectionLink(sectionName: SectionName, size: targetSize);
            base.Draw(availableSpace: availableSpace);
        }
    }
}