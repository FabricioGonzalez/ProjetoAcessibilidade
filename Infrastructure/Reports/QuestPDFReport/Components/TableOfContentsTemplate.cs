using QuestPDF.Fluent;
using QuestPDF.Helpers;
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
                        .Text(text: "Table of contents")
                        .Style(style: Typography.Headline);

                    decoration.Content().Column(handler: column =>
                    {
                        column.Spacing(value: 5);

                        for (var i = 0; i < Sections.Count; i++)
                        {
                            if (Sections[index: i] is ReportSection reportSection)
                            {
                                column.Item().Element(handler: c =>
                                    DrawLink(container: c, number: i + 1, locationName: reportSection.Title));
                            }

                            if (Sections[index: i] is ReportSectionGroup reportSectionGroup)
                            {
                                column.Item().Column(handler: colItem =>
                                {
                                    colItem.Item()
                                        .Element(handler: c => DrawLink(container: c, number: i + 1
                                            , locationName: reportSectionGroup.Title));

                                    for (var part = 0; part < reportSectionGroup.Parts.Count; part++)
                                    {
                                        colItem.Item()
                                            .Element(
                                                handler: c => DrawDeepLink(container: c, parent: i + 1, number: part + 1
                                                    , locationName: reportSectionGroup.Parts[index: part].Title));
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
        , string locationName
    ) =>
        container
            .SectionLink(sectionName: locationName)
            .Row(handler: row =>
            {
                row.ConstantItem(size: 20).Text(text: $"{number}.");
                row.AutoItem().Text(text: locationName);

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
                            StrokeWidth = space.Height
                            , PathEffect = SKPathEffect.CreateDash(intervals: new float[] { 1, 3 }, phase: 0)
                        };

                        canvas.DrawLine(x0: 0, y0: 0, x1: space.Width, y1: 0, paint: paint);
                    });

                row.AutoItem().Text(content: text =>
                {
                    text.BeginPageNumberOfSection(locationName: locationName);
                    text.Span(text: " - ");
                    text.EndPageNumberOfSection(locationName: locationName);

                    var lengthStyle = TextStyle.Default.FontColor(value: Colors.Grey.Medium);

                    text.TotalPagesWithinSection(locationName: locationName).Style(style: lengthStyle).Format(
                        formatter: x =>
                        {
                            var formatted = x == 1 ? "1 page long" : $"{x} pages long";
                            return $" ({formatted})";
                        });
                });
            });

    private void DrawDeepLink(
        IContainer container
        , int parent
        , int number
        , string locationName
    ) =>
        container
            .PaddingLeft(value: 8)
            .SectionLink(sectionName: locationName)
            .Row(handler: row =>
            {
                row.ConstantItem(size: 20).Text(text: $"{parent}.{number}.");
                row.AutoItem().Text(text: locationName);

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
                            StrokeWidth = space.Height
                            , PathEffect = SKPathEffect.CreateDash(intervals: new float[] { 1, 3 }, phase: 0)
                        };

                        canvas.DrawLine(x0: 0, y0: 0, x1: space.Width, y1: 0, paint: paint);
                    });

                row.AutoItem().Text(content: text =>
                {
                    text.BeginPageNumberOfSection(locationName: locationName);
                    text.Span(text: " - ");
                    text.EndPageNumberOfSection(locationName: locationName);

                    var lengthStyle = TextStyle.Default.FontColor(value: Colors.Grey.Medium);

                    text.TotalPagesWithinSection(locationName: locationName).Style(style: lengthStyle).Format(
                        formatter: x =>
                        {
                            var formatted = x == 1 ? "1 page long" : $"{x} pages long";
                            return $" ({formatted})";
                        });
                });
            });
}