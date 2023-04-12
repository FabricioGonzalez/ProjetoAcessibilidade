using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Elements
{
    public class DynamicImage : Element
    {
        public Func<Size, byte[]>? Source
        {
            get;
            set;
        }

        public override SpacePlan Measure(
            Size availableSpace
        ) => SpacePlan.FullRender(width: availableSpace.Width, height: availableSpace.Height);

        public override void Draw(
            Size availableSpace
        )
        {
            var imageData = Source?.Invoke(arg: availableSpace);

            if (imageData == null)
            {
                return;
            }

            var imageElement = new Image
            {
                publicImage = SKImage.FromEncodedData(data: imageData)
            };

            imageElement.Initialize(pageContext: PageContext, canvas: Canvas);
            imageElement.Draw(availableSpace: availableSpace);
        }
    }
}