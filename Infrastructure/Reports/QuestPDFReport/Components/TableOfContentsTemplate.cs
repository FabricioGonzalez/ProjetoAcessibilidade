using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;
using SkiaSharp;

namespace QuestPDFReport.Components;

public class TableOfContentsTemplate : IComponent
{
    public TableOfContentsTemplate(
        List<IReportSection>? sections = null
    )
    {
        Sections = sections;
    }

    private List<IReportSection>? Sections
    {
        get;
    }

    public void Compose(
        IContainer container
    )
    {
        try
        {
            container
                .Decoration(decoration =>
                {
                    decoration
                        .Before()
                        .PaddingBottom(5)
                        .AlignCenter()
                        .Text("Sumário")
                        .Style(Typography.Headline);

                    decoration.Content().Column(column =>
                    {
                        column.Spacing(5);

                        for (var i = 0; i < Sections.Count; i++)
                        {
                            if (Sections[i] is ReportSectionGroup reportSectionGroup)
                            {
                                column.Item().Column(colItem =>
                                {
                                    colItem.Item()
                                        .Element(con => DrawLink(con, i + 1
                                            , reportSectionGroup.Title,
                                            reportSectionGroup.Title));

                                    for (var part = 0; part < reportSectionGroup.Parts.Count; part++)
                                    {
                                        colItem.Item()
                                            .Element(
                                                c => DrawDeepLink(c, i + 1, part + 1
                                                    , reportSectionGroup.Parts[part].Title,
                                                    $"{reportSectionGroup.Title} - {reportSectionGroup.Parts[part].Title}"));
                                    }
                                });
                            }
                        }
                    });
                });
        }
        catch (ArgumentNullException)
        {
        }
    }

    private void DrawLink(
        IContainer container
        , int number
        , string itemName
        , string id
    ) =>
        container
            .SectionLink(id)
            .Row(row =>
            {
                row.ConstantItem(20).Text($"{number}.");
                row.AutoItem().Text(itemName);

                row.RelativeItem()
                    .PaddingHorizontal(2)
                    .AlignBottom()
                    .TranslateY(-3)
                    .Height(1)
                    .Canvas((
                        canvas
                        , space
                    ) =>
                    {
                        // best to statically cache
                        using var paint = new SKPaint
                        {
                            StrokeWidth = space.Height, PathEffect = SKPathEffect.CreateDash(new float[] { 1, 3 }, 0)
                        };

                        canvas.DrawLine(0, 0, space.Width, 0, paint);
                    });

                row.ConstantItem(40).Text(text =>
                {
                    text.BeginPageNumberOfSection(id);
                    text.Span(" - ");
                    text.EndPageNumberOfSection(id);
                });
            });

    private void DrawDeepLink(
        IContainer container
        , int parent
        , int number
        , string itemName
        , string nestedItemId
    ) =>
        container
            .PaddingLeft(8)
            .SectionLink(nestedItemId)
            .Row(row =>
            {
                row.ConstantItem(20).Text($"{parent}.{number}.");
                row.AutoItem().Text(itemName);

                row.RelativeItem()
                    .PaddingHorizontal(4)
                    .AlignBottom()
                    .TranslateY(-3)
                    .Height(1)
                    .Canvas((
                        canvas
                        , space
                    ) =>
                    {
                        // best to statically cache
                        using var paint = new SKPaint
                        {
                            StrokeWidth = space.Height, PathEffect = SKPathEffect.CreateDash(new float[] { 1, 3 }, 0)
                        };

                        canvas.DrawLine(0, 0, space.Width, 0, paint);
                    });

                row.ConstantItem(40).Text(text =>
                {
                    text.BeginPageNumberOfSection(nestedItemId);
                    text.Span(" - ");
                    text.EndPageNumberOfSection(nestedItemId);
                });
            });
}