using SkiaSharp;

namespace QuestPDF.Previewer;

internal class Helpers
{
    public static void GeneratePdfFromDocumentSnapshots(
        string filePath
        , ICollection<PreviewPage> pages
    )
    {
        using var stream = File.Create(path: filePath);

        using var document = SKDocument.CreatePdf(stream: stream);

        foreach (var page in pages)
        {
            using var canvas = document.BeginPage(width: page.Width, height: page.Height);
            canvas.DrawPicture(picture: page.Picture);
            document.EndPage();
        }
    }
}