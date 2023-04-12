using System.IO;
using SkiaSharp;

namespace QuestPDF.Drawing
{
    public class XpsCanvas : SkiaDocumentCanvasBase
    {
        public XpsCanvas(
            Stream stream
            , DocumentMetadata documentMetadata
        )
            : base(document: SKDocument.CreateXps(stream: stream, dpi: documentMetadata.RasterDpi))
        {
        }
    }
}