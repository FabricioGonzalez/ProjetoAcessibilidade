using System.IO;
using SkiaSharp;

namespace QuestPDF.Drawing
{
    public class PdfCanvas : SkiaDocumentCanvasBase
    {
        public PdfCanvas(
            Stream stream
            , DocumentMetadata documentMetadata
        )
            : base(document: SKDocument.CreatePdf(stream: stream, metadata: MapMetadata(metadata: documentMetadata)))
        {
        }

        private static SKDocumentPdfMetadata MapMetadata(
            DocumentMetadata metadata
        ) =>
            new()
            {
                Title = metadata.Title, Author = metadata.Author, Subject = metadata.Subject
                , Keywords = metadata.Keywords, Creator = metadata.Creator, Producer = metadata.Producer
                , Creation = metadata.CreationDate, Modified = metadata.ModifiedDate, RasterDpi = metadata.RasterDpi
                , EncodingQuality = metadata.ImageQuality, PdfA = metadata.PdfA
            };
    }
}