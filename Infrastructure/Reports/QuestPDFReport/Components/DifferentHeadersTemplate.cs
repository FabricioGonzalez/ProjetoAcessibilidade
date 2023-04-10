using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDFReport.Components;

public class DifferentHeadersTemplate : IDocument
{
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(
        IDocumentContainer container
    ) =>
        container
            .Page(handler: page =>
            {
                page.Margin(value: 40);

                page.Size(pageSize: PageSizes.A4);

                page.Header().Element(handler: ComposeHeader);
                page.Content().Element(handler: ComposeContent);
                page.Footer().Element(handler: ComposeFooter);
            });

    private void ComposeHeader(
        IContainer container
    ) =>
        container.Background(color: Colors.Grey.Lighten3).Border(value: 1).Column(handler: column =>
        {
            column.Item().ShowOnce().Padding(value: 5).AlignMiddle().Row(handler: row =>
            {
                row.RelativeItem(size: 2).AlignMiddle().Text(text: "PRIMARY HEADER")
                    .FontColor(value: Colors.Grey.Darken3).FontSize(value: 30).Bold();
                row.RelativeItem().AlignRight().MinimalBox().AlignMiddle().Background(color: Colors.Blue.Darken2)
                    .Padding(value: 30);
            });
            column.Item().SkipOnce().Padding(value: 5).Row(handler: row =>
            {
                row.RelativeItem(size: 2).Text(text: "SECONDARY HEADER").FontColor(value: Colors.Grey.Darken3)
                    .FontSize(value: 30).Bold();
                row.RelativeItem().AlignRight().MinimalBox().Background(color: Colors.Blue.Lighten4).Padding(value: 15);
            });
        });

    private void ComposeContent(
        IContainer container
    ) =>
        container.Column(handler: column =>
        {
            column.Item().PaddingVertical(value: 80).Text(text: "First");
            column.Item().PageBreak();
            column.Item().PaddingVertical(value: 80).Text(text: "Second");
            column.Item().PageBreak();
            column.Item().PaddingVertical(value: 80).Text(text: "Third");
            column.Item().PageBreak();
        });

    private void ComposeFooter(
        IContainer container
    ) =>
        container.Background(color: Colors.Grey.Lighten3).Column(handler: column =>
        {
            column.Item().ShowOnce().Background(color: Colors.Grey.Lighten3).Row(handler: row =>
            {
                row.RelativeItem().Text(content: x =>
                {
                    x.CurrentPageNumber();
                    x.Span(text: " / ");
                    x.TotalPages();
                });
                row.RelativeItem().AlignRight().Text(text: "Footer for header");
            });

            column.Item().SkipOnce().Background(color: Colors.Grey.Lighten3).Row(handler: row =>
            {
                row.RelativeItem().Text(content: x =>
                {
                    x.CurrentPageNumber();
                    x.Span(text: " / ");
                    x.TotalPages();
                });
                row.RelativeItem().AlignRight().Text(text: "Footer for every page except header");
            });
        });
}