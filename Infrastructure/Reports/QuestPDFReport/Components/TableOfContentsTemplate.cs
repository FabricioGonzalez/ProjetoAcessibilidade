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
                .Decoration(handler: decoration =>
                {
                    decoration
                        .Before()
                        .PaddingBottom(value: 5)
                        .Text(text: "Sumário")
                        .Style(style: Typography.Headline);

                    decoration.Content().Column(handler: column =>
                    {
                        column.Spacing(value: 5);

                        for (var i = 0; i < Sections.Count; i++)
                        {
                            /*if (Sections[index: i] is ReportSection reportSection)
                            {
                                column.Item().Element(handler: c =>
                                    DrawLink(container: c, number: i + 1, itemName: reportSection.Title,
                                        id: reportSection.Id));
                            }*/

                            if (Sections[index: i] is ReportSectionGroup reportSectionGroup)
                            {
                                column.Item().Column(handler: colItem =>
                                {
                                    colItem.Item()
                                        .Element(handler: c => DrawLink(container: c, number: i + 1
                                            , itemName: reportSectionGroup.Title,
                                            id: reportSectionGroup.Id));

                                    for (var part = 0; part < reportSectionGroup.Parts.Count; part++)
                                    {
                                        colItem.Item()
                                            .Element(
                                                handler: c => DrawDeepLink(container: c, parent: i + 1, number: part + 1
                                                    , itemName: reportSectionGroup.Parts[index: part].Title,
                                                    nestedItemId: reportSectionGroup.Parts[index: part].Id));
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
        , string itemName,
        string id
    ) =>
        container
            .SectionLink(sectionName: id)
            .Row(handler: row =>
            {
                row.ConstantItem(size: 20).Text(text: $"{number}.");
                row.AutoItem().Text(text: itemName);

                row.RelativeItem()
                    .PaddingHorizontal(value: 2)
                    .AlignBottom()
                    .TranslateY(value: -3)
                    .Height(value: 1)
                    .Canvas(handler: (
                        canvas
                        , space
                    ) =>
                    {
                        // best to statically cache
                        using var paint = new SKPaint
                        {
                            StrokeWidth = space.Height,
                            PathEffect = SKPathEffect.CreateDash(intervals: new float[] { 1, 3 }, phase: 0)
                        };

                        canvas.DrawLine(x0: 0, y0: 0, x1: space.Width, y1: 0, paint: paint);
                    });

                row.ConstantItem(size: 40).Text(content: text =>
                {
                    text.BeginPageNumberOfSection(locationName: id);
                    text.Span(text: " - ");
                    text.EndPageNumberOfSection(locationName: id);
                });
            });

    private void DrawDeepLink(
        IContainer container
        , int parent
        , int number
        , string itemName,
        string nestedItemId
    ) =>
        container
            .PaddingLeft(value: 8)
            .SectionLink(sectionName: nestedItemId)
            .Row(handler: row =>
            {
                row.ConstantItem(size: 20).Text(text: $"{parent}.{number}.");
                row.AutoItem().Text(text: itemName);

                row.RelativeItem()
                    .PaddingHorizontal(value: 4)
                    .AlignBottom()
                    .TranslateY(value: -3)
                    .Height(value: 1)
                    .Canvas(handler: (
                        canvas
                        , space
                    ) =>
                    {
                        // best to statically cache
                        using var paint = new SKPaint
                        {
                            StrokeWidth = space.Height,
                            PathEffect = SKPathEffect.CreateDash(intervals: new float[] { 1, 3 }, phase: 0)
                        };

                        canvas.DrawLine(x0: 0, y0: 0, x1: space.Width, y1: 0, paint: paint);
                    });

                row.ConstantItem(size: 40).Text(content: text =>
                {
                    text.BeginPageNumberOfSection(locationName: nestedItemId);
                    text.Span(text: " - ");
                    text.EndPageNumberOfSection(locationName: nestedItemId);
                });
            });
}