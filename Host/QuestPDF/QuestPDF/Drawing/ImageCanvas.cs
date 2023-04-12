using System.Collections.Generic;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Drawing
{
    public class ImageCanvas : SkiaCanvasBase
    {
        public ImageCanvas(
            DocumentMetadata metadata
        )
        {
            Metadata = metadata;
        }

        private DocumentMetadata Metadata
        {
            get;
        }

        private SKSurface Surface
        {
            get;
            set;
        }

        public ICollection<byte[]> Images
        {
            get;
        } = new List<byte[]>();

        public override void BeginDocument()
        {
        }

        public override void EndDocument()
        {
            Canvas?.Dispose();
            Surface?.Dispose();
        }

        public override void BeginPage(
            Size size
        )
        {
            var scalingFactor = Metadata.RasterDpi / (float)PageSizes.PointsPerInch;
            var imageInfo = new SKImageInfo(width: (int)(size.Width * scalingFactor)
                , height: (int)(size.Height * scalingFactor));

            Surface = SKSurface.Create(info: imageInfo);
            Canvas = Surface.Canvas;

            Canvas.Scale(s: scalingFactor);
        }

        public override void EndPage()
        {
            Canvas.Save();
            var image = Surface.Snapshot().Encode(format: SKEncodedImageFormat.Png, quality: 100).ToArray();
            Images.Add(item: image);

            Canvas.Dispose();
            Surface.Dispose();
        }
    }
}