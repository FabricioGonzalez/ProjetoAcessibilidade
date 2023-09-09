using Common.Linq;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;
using SkiaSharp;

namespace QuestPDFReport.Components;

public class TableOfContentsTemplate : IComponent
{
    public TableOfContentsTemplate(
        List<ReportLocationGroup> sections = null
    )
    {
        Locations = sections;
    }

    private List<ReportLocationGroup>? Locations
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

                    decoration.Content()
                    .Column(column =>
                    {
                        column.Spacing(5);

                        Locations.IterateOn(it =>
                        {
                            column.Item().Column(colItem =>
                            {
                                var itemIndex = Locations.IndexOf(it) + 1;
                                colItem.Item()
                                    .Element(con => DrawLink(con, itemIndex
                                        , it.Title,
                                        it.Title));

                                it.Groups.IterateOn(group =>
                                {
                                    var groupIndex = it.Groups.IndexOf(group) + 1;

                                    colItem.Item()
                                        .Element(
                                            c => DrawDeepLink(c, $"{itemIndex}.{groupIndex}"
                                                , group.Title,
                                                $"{it.Title} - {group.Title}"));

                                    group.Parts.IterateOn(part =>
                                    {
                                        var partIndex = group.Parts.IndexOf(part) + 1;
                                        colItem.Item()
                                            .Element(
                                                c => DrawDeepLink(c, $"{itemIndex}.{groupIndex}.{partIndex}"
                                                    , part.Title,
                                                    $"{it.Title} - {group.Title} - {part.Title}"));
                                    });
                                });
                            });
                        });

                        column.Item()
                                    .Element(con => DrawLink(con, Locations.Count + 1
                                        , "Conclusão",
                                        "conclusao"));
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
        , string pageNumber
        , string itemName
        , string nestedItemId
    ) =>
        container
            .PaddingLeft(8)
            .SectionLink(nestedItemId)
            .Row(row =>
            {
                row.RelativeItem(0.1f).Text($"{pageNumber}.");
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