using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProjectAvalonia.Features.PDFViewer;

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
            page.Size(pageSize: PageSizes.A4);
            page.Margin(value: 1, unit: Unit.Inch);
            page.PageColor(color: Colors.Red.Lighten4);
            page.DefaultTextStyle(handler: x => x.FontSize(value: 16));

            page.Header()
                .BorderBottom(value: 2)
                .BorderColor(color: Colors.Red.Medium)
                .PaddingBottom(value: 5)
                .Text(text: "Ooops! Something went wrong...").FontSize(value: 28).FontColor(Colors.Red.Medium)
                .Bold();

            page.Content().PaddingVertical(value: 20).Column(handler: column =>
            {
                var currentException = Exception;

                while (currentException != null)
                {
                    column.Item().Text(text: currentException.GetType().Name).FontSize(value: 20).SemiBold();
                    column.Item().Text(text: currentException.Message).FontSize(value: 14);
                    column.Item().PaddingTop(value: 10).Text(text: currentException.StackTrace).FontSize(value: 10)
                        .Light();

                    currentException = currentException.InnerException;

                    if (currentException != null)
                    {
                        column.Item().PaddingVertical(value: 15).LineHorizontal(size: 2)
                            .LineColor( Colors.Red.Medium);
                    }
                }
            });
        });
}