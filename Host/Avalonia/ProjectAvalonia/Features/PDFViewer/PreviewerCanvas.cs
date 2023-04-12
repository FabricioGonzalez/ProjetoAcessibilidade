using System.Collections.Generic;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Previewer;

public record PreviewPage(
    SKPicture Picture
    , Size Size
);

public sealed class PreviewerCanvas
    : SkiaCanvasBase
        , IRenderingCanvas
{
    private SKPictureRecorder? PictureRecorder
    {
        get;
        set;
    }

    private Size? CurrentPageSize
    {
        get;
        set;
    }

    public ICollection<PreviewPage> Pictures
    {
        get;
    } = new List<PreviewPage>();

    public override void BeginDocument() => Pictures.Clear();

    public override void BeginPage(
        Size size
    )
    {
        CurrentPageSize = size;
        PictureRecorder = new SKPictureRecorder();

        Canvas = PictureRecorder.BeginRecording(cullRect: new SKRect(left: 0, top: 0, right: size.Width
            , bottom: size.Height));
    }

    public override void EndPage()
    {
        var picture = PictureRecorder?.EndRecording();

        if (picture != null && CurrentPageSize.HasValue)
        {
            Pictures.Add(item: new PreviewPage(Picture: picture, Size: CurrentPageSize.Value));
        }

        PictureRecorder?.Dispose();
        PictureRecorder = null;
    }

    public override void EndDocument()
    {
    }
}