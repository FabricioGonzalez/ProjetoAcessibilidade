using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Elements
{
    public class DynamicImage : Element
    {
        public Func<Size, byte[]>? Source { get; set; }
        
        public override SpacePlan Measure(Size availableSpace)
        {
            return SpacePlan.FullRender(availableSpace.Width, availableSpace.Height);
        }

        public override void Draw(Size availableSpace)
        {
            var imageData = Source?.Invoke(availableSpace);
            
            if (imageData == null)
                return;

            var imageElement = new Image
            {
                publicImage = SKImage.FromEncodedData(imageData)
            };
            
            imageElement.Initialize(PageContext, Canvas);
            imageElement.Draw(availableSpace);
        }
    }
}