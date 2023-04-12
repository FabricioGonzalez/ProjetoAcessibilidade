using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Hyperlink : ContainerElement
    {
        public string Url
        {
            get;
            set;
        } = "https://www.questpdf.com";

        public override void Draw(
            Size availableSpace
        )
        {
            var targetSize = base.Measure(availableSpace: availableSpace);

            if (targetSize.Type == SpacePlanType.Wrap)
            {
                return;
            }

            Canvas.DrawHyperlink(url: Url, size: targetSize);
            base.Draw(availableSpace: availableSpace);
        }
    }
}