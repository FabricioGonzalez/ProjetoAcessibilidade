﻿using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using QuestPDFReport.Models;

using SkiaSharp;

namespace QuestPDFReport.Components;
public class TableOfContentsTemplate : IComponent
{
    private List<IReportSection>? Sections
    {
        get;
    }

    public TableOfContentsTemplate(List<IReportSection>? sections = null)
    {
        Sections = sections;
    }

    public void Compose(IContainer container)
    {
        try
        {
            container
           .Decoration(decoration =>
           {
               decoration
                   .Before()
                   .PaddingBottom(5)
                   .Text("Table of contents")
                   .Style(Typography.Headline);

               decoration.Content().Column(column =>
               {
                   column.Spacing(5);

                   for (var i = 0; i < Sections.Count; i++)
                   {

                       if (Sections[i] is ReportSection reportSection)
                       {
                           column.Item().Element(c => DrawLink(c, i + 1, reportSection.Title));
                       }
                       if (Sections[i] is ReportSectionGroup reportSectionGroup)
                       {
                           column.Item().Element(c => DrawLink(c, i + 1, reportSectionGroup.Title));
                       }
                   }
               });
           });
        }
        catch (ArgumentNullException)
        {
            return;
        }
        catch (Exception)
        {
            throw;
        }

    }

    private void DrawLink(IContainer container, int number, string locationName)
    {
        container
            .SectionLink(locationName)
            .Row(row =>
            {
                row.ConstantItem(20).Text($"{number}.");
                row.AutoItem().Text(locationName);

                row.RelativeItem().PaddingHorizontal(2).AlignBottom().TranslateY(-3).Height(1).Canvas((canvas, space) =>
                {
                    // best to statically cache
                    using var paint = new SKPaint
                    {
                        StrokeWidth = space.Height,
                        PathEffect = SKPathEffect.CreateDash(new float[] { 1, 3 }, 0)
                    };

                    canvas.DrawLine(0, 0, space.Width, 0, paint);
                });

                row.AutoItem().Text(text =>
                {
                    text.BeginPageNumberOfSection(locationName);
                    text.Span(" - ");
                    text.EndPageNumberOfSection(locationName);

                    var lengthStyle = TextStyle.Default.FontColor(Colors.Grey.Medium);

                    text.TotalPagesWithinSection(locationName).Style(lengthStyle).Format(x =>
                    {
                        var formatted = x == 1 ? "1 page long" : $"{x} pages long";
                        return $" ({formatted})";
                    });
                });
            });
    }
}

