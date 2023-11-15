using SkiaSharp;

namespace QuestPDF.Previewer;

internal record PreviewPage(
    SKPicture Picture
    , float Width
    , float Height
);