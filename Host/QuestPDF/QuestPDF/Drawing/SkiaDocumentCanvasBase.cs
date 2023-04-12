using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Drawing
{
    public class SkiaDocumentCanvasBase : SkiaCanvasBase
    {
        protected SkiaDocumentCanvasBase(
            SKDocument document
        )
        {
            Document = document;
        }

        private SKDocument? Document
        {
            get;
        }

        ~SkiaDocumentCanvasBase()
        {
            Document?.Dispose();
        }

        public override void BeginDocument()
        {
        }

        public override void EndDocument()
        {
            Canvas?.Dispose();

            Document.Close();
            Document.Dispose();
        }

        public override void BeginPage(
            Size size
        ) => Canvas = Document.BeginPage(width: size.Width, height: size.Height);

        public override void EndPage()
        {
            Document.EndPage();
            Canvas.Dispose();
        }
    }
}