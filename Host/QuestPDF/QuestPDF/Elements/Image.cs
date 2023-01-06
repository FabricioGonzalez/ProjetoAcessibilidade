using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Elements
{
    public class Image : Element, ICacheable
    {
        public SKImage? publicImage { get; set; }

        ~Image()
        {
            publicImage?.Dispose();
        }
        
        public override SpacePlan Measure(Size availableSpace)
        {
            return SpacePlan.FullRender(availableSpace);
        }

        public override void Draw(Size availableSpace)
        {
            if (publicImage == null)
                return;

            Canvas.DrawImage(publicImage, Position.Zero, availableSpace);
        }
    }
}