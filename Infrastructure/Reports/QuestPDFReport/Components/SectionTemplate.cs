using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using QuestPDFReport.Models;

using SkiaSharp;

namespace QuestPDFReport.Components;
public class SectionTemplate : IComponent
{
    public ReportSection Model
    {
        get; set;
    }

    public SectionTemplate(ReportSection model)
    {
        Model = model;
    }

    public void Compose(IContainer container)
    {
        container
            .EnsureSpace()
            .Decoration(decoration =>
            {
                decoration
                    .Before()
                    .PaddingBottom(5)
                    .Text(Model.Title)
                    .Style(Typography.Headline);

                decoration.Content().Border(0.75f).BorderColor(Colors.Grey.Medium).Column(column =>
                {
                    foreach (var part in Model.Parts)
                    {
                        column.Item().EnsureSpace(25).Row(row =>
                        {
                            row.ConstantItem(150).LabelCell().Text(part.Label);
                            var frame = row.RelativeItem().ValueCell();

                            if (part is ReportSectionText text)
                                frame.ShowEntire().Text(text.Text);

                            if (part is ReportSectionCheckbox checkboxes)
                            {
                                frame.Element(x => MapCheckboxes(x, checkboxes));
                            }


                            if (part is ReportSectionMap map)
                                frame.Element(x => MapElement(x, map));

                            if (part is ReportSectionPhotos photos)
                                frame.Element(x => PhotosElement(x, photos));
                        });
                    }
                });
            });
    }

    private void MapCheckboxes(IContainer container, ReportSectionCheckbox checkboxes)
    {
        container.ShowEntire().Column(column =>
        {
            column.Spacing(5);
            column.Item().Row(row =>
            {
                foreach (var item in checkboxes.Checkboxes)
                {
                    row.Spacing(5);

                    row.ConstantItem(16)
                    .Layers(layers =>
                    {
                        layers.Layer().Canvas((canvas, size) =>
                        {
                            DrawRoundedRectangle(Colors.White, false);
                            DrawRoundedRectangle(Colors.Blue.Darken2, true);

                            if (item.IsChecked)
                            {
                                DrawLine(Colors.Blue.Darken2, false, fromX: 2, fromY: 2, toX: 8, toY: 15);
                                DrawLine(Colors.Blue.Darken2, false, fromX: 8, fromY: 15f, toX: 14, toY: -3.5f);
                            }


                            void DrawLine(string color, bool isStroke, float fromX, float fromY, float toX, float toY)
                            {
                                using var paint = new SKPaint
                                {
                                    Color = SKColor.Parse(color),
                                    IsStroke = isStroke,
                                    StrokeWidth = 1,
                                    IsAntialias = true
                                };

                                canvas.DrawLine(new SKPoint(fromX, fromY), new SKPoint(toX, toY), paint);
                            }

                            void DrawRoundedRectangle(string color, bool isStroke)
                            {
                                using var paint = new SKPaint
                                {
                                    Color = SKColor.Parse(color),
                                    IsStroke = isStroke,
                                    StrokeWidth = 1,
                                    IsAntialias = true
                                };

                                canvas.DrawRoundRect(0, 0, 16, 16, 4, 8, paint);
                            }
                        });

                        layers
                            .PrimaryLayer()
                                                                                              /* .Text("Sample text")
                                                                                               .FontSize(16).FontColor(Colors.Blue.Darken2).SemiBold()*/;
                    });

                    row.AutoItem().Text(item.Value);
                }
            });

        });
    }

    void MapElement(IContainer container, ReportSectionMap model)
    {
        if (model.Location == null)
        {
            container.Text("No location provided");
            return;
        }

        container.ShowEntire().Column(column =>
        {
            column.Spacing(5);

            column.Item().MaxWidth(250).AspectRatio(4 / 3f).Component<ImagePlaceholder>();
            column.Item().Text(model.Location.Format());
        });
    }

    void PhotosElement(IContainer container, ReportSectionPhotos model)
    {
        if (model.PhotoCount == 0)
        {
            container.Text("No photos").Style(Typography.Normal);
            return;
        }

        container.DebugArea("Photos").Grid(grid =>
        {
            grid.Spacing(5);
            grid.Columns(3);

            Enumerable
                .Range(0, model.PhotoCount)
                .ToList()
                .ForEach(x => grid.Item().AspectRatio(4 / 3f).Component<ImagePlaceholder>());
        });
    }
}
