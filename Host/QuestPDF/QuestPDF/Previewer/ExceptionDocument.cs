using System;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Previewer
{
    public class ExceptionDocument : IDocument
    {
        public ExceptionDocument(
            Exception exception
        )
        {
            Exception = exception;
        }

        private Exception Exception
        {
            get;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(
            IDocumentContainer document
        ) =>
            document.Page(handler: page =>
            {
                page.ContinuousSize(width: PageSizes.A4.Width);
                page.Margin(value: 50);
                page.DefaultTextStyle(handler: x => x.FontSize(value: 16));

                page.Foreground().PaddingTop(value: 5).Border(value: 10).BorderColor(color: Colors.Red.Medium);

                page.Header()
                    .ShowOnce()
                    .PaddingBottom(value: 5)
                    .Row(handler: row =>
                    {
                        row.Spacing(value: 15);

                        row.AutoItem()
                            .PaddingTop(value: 15)
                            .Width(value: 48)
                            .AspectRatio(ratio: 1)
                            .Canvas(handler: (
                                canvas
                                , size
                            ) =>
                            {
                                const float iconSize = 24;
                                using var iconPath = SKPath.ParseSvgPathData(
                                    svgPath:
                                    "M23,12L20.56,14.78L20.9,18.46L17.29,19.28L15.4,22.46L12,21L8.6,22.47L6.71,19.29L3.1,18.47L3.44,14.78L1,12L3.44,9.21L3.1,5.53L6.71,4.72L8.6,1.54L12,3L15.4,1.54L17.29,4.72L20.9,5.54L20.56,9.22L23,12M20.33,12L18.5,9.89L18.74,7.1L16,6.5L14.58,4.07L12,5.18L9.42,4.07L8,6.5L5.26,7.09L5.5,9.88L3.67,12L5.5,14.1L5.26,16.9L8,17.5L9.42,19.93L12,18.81L14.58,19.92L16,17.5L18.74,16.89L18.5,14.1L20.33,12M11,15H13V17H11V15M11,7H13V13H11V7");

                                using var paint = new SKPaint
                                {
                                    Color = SKColors.Red, IsAntialias = true
                                };

                                canvas.Scale(s: size.Width / iconSize);
                                canvas.DrawPath(path: iconPath, paint: paint);
                            });

                        row.RelativeItem()
                            .Column(handler: column =>
                            {
                                column.Item().Text(text: "Exception").FontSize(value: 36)
                                    .FontColor(value: Colors.Red.Medium).Bold();
                                column.Item().PaddingTop(value: -10)
                                    .Text(text: "Don't panic! Just analyze what's happened...").FontSize(value: 18)
                                    .FontColor(value: Colors.Red.Medium).Bold();
                            });
                    });

                page.Content().PaddingVertical(value: 20).Column(handler: column =>
                {
                    var currentException = Exception;

                    while (currentException != null)
                    {
                        column.Item()
                            .PaddingTop(value: 25)
                            .PaddingBottom(value: 15)
                            .Padding(value: -10)
                            .Background(color: Colors.Grey.Lighten3)
                            .Padding(value: 10)
                            .Text(content: text =>
                            {
                                text.DefaultTextStyle(style: x => x.FontSize(value: 16));

                                text.Span(text: currentException.GetType().Name + ": ").Bold();
                                text.Span(text: currentException.Message);
                            });

                        foreach (var trace in currentException.StackTrace.Split(separator: '\n'))
                        {
                            column
                                .Item()
                                .ShowEntire()
                                .BorderBottom(value: 1)
                                .BorderColor(color: Colors.Grey.Lighten2)
                                .PaddingVertical(value: 5)
                                .Text(text: trace.Trim())
                                .FontSize(value: 12);
                        }

                        currentException = currentException.InnerException;
                    }
                });
            });
    }
}