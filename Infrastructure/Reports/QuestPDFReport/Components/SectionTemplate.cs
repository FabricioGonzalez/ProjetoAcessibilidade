using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using QuestPDFReport.Models;

using SkiaSharp;

namespace QuestPDFReport.Components;
public class SectionTemplate : IComponent
{
    public IReportSection Model
    {
        get; set;
    }

    public SectionTemplate(IReportSection model)
    {
        Model = model;
    }

    public void Compose(IContainer container)
    {
        if (Model is ReportSection reportSection)
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

                    decoration
                    .Content()
                    .Border(0.75f)
                    .BorderColor(Colors.Grey.Medium)
                    .Column(column =>
                    {
                        foreach (var part in reportSection.Parts)
                        {
                            column
                            .Item()
                            .EnsureSpace(25)
                            .Column(column =>
                            {
                                column
                                .Item()
                                .LabelCell()
                                .ExtendHorizontal()
                                .Text(part.Label);

                                if (part is not ReportSectionTitle)
                                {
                                    var frame = column
                                    .Item()
                                    .ValueCell();

                                    if (part is ReportSectionText text)
                                    {
                                        frame
                                    .ShowEntire()
                                    .Text(text.Text);
                                    }

                                    if (part is ReportSectionCheckbox checkboxes)
                                    {
                                        frame
                                    .Element(x => MapCheckboxes(x, checkboxes));
                                    }

                                    if (part is ReportSectionPhotoContainer photos)
                                    {
                                        frame
                                        .Element(x => PhotosElement(x, photos));
                                    }

                                    if (part is ReportSectionObservation observation)
                                    {
                                        frame
                                         .Background(Colors.Yellow.Medium)
                                        .Element(x => ObservationElement(x, observation));
                                    }
                                }
                            });
                        }
                    });
                });
        }

        if (Model is ReportSectionGroup reportSectionGroup)
        {
            container
                .EnsureSpace()
                .Decoration(decoration =>
                {
                    decoration
                        .Before()
                        .PaddingBottom(5)
                        .ShowOnce()
                        .Text(Model.Title)
                        .Style(Typography.Headline);

                    decoration
                    .Content()
                    .Border(0.75f)
                    .BorderColor(Colors.Grey.Medium)
                    .Column(column =>
                    {

                        foreach (var part in reportSectionGroup.Parts)
                        {
                            column
                            .Item()
                            .PaddingLeft(8)
                            .ShowOnce()
                            .Text(part.Title)
                            .Style(Typography.SubLine);

                            column.Item().Column(items =>
                        {
                            foreach (var part in part.Parts)
                            {
                                column
                                .Item()
                                .ValueCell()
                                .EnsureSpace(25)
                                .Column(column =>
                                {
                                    column
                                    .Item()
                                    .LabelCell()
                                    .ExtendHorizontal()
                                    .Text(part.Label);

                                    if (part is not ReportSectionTitle)
                                    {
                                        var frame = column
                                        .Item()
                                        .ValueCell();

                                        if (part is ReportSectionText text)
                                        {
                                            frame
                                        .ShowEntire()
                                        .Text(text.Text);
                                        }

                                        if (part is ReportSectionCheckbox checkboxes)
                                        {
                                            frame
                                        .Element(x => MapCheckboxes(x, checkboxes));
                                        }

                                        if (part is ReportSectionPhotoContainer photos)
                                        {
                                            frame
                                            .Element(x => PhotosElement(x, photos));
                                        }

                                        if (part is ReportSectionObservation observation)
                                        {
                                            frame
                                             .Background(Colors.Yellow.Medium)
                                            .Element(x => ObservationElement(x, observation));
                                        }
                                    }
                                });
                            }
                        });
                        }
                    });
                });
        }
    }

    private void ObservationElement(IContainer x, ReportSectionObservation observation)
    {
        x
            .Text(observation.Observation)
            .Style(Typography.Normal);
    }

    private void MapTitle(IContainer container, string title)
    {
        container.ShowEntire().Column(column =>
        {
            column.Item().Text(title).Style(Typography.Normal);
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
                            DrawRoundedRectangle(Colors.Black, true);

                            if (item.IsChecked)
                            {
                                DrawLine(Colors.Black, true, fromX: 6, fromY: 4, toX: 10, toY: 12);
                                DrawLine(Colors.Black, true, fromX: 9.80f, fromY: 12.0f, toX: 15.5f, toY: -1f);
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

                                canvas.DrawRoundRect(4, 2, 12, 12, 2, 4, paint);
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

    private void MapElement(IContainer container, ReportSectionMap model)
    {
        if (model.Location == null)
        {
            container.Text("No location provided");
            return;
        }

        container
            .ShowEntire()
            .Column(column =>
        {
            column
            .Spacing(5);

            column
            .Item()
            .MaxWidth(250)
            .AspectRatio(4 / 3f)
            .Component<ImagePlaceholder>();

            column
            .Item()
            .Text(model.Location.Format());
        });
    }

    private void PhotosElement(IContainer container, ReportSectionPhotoContainer model)
    {
        if (model.Photos.Count == 0)
        {
            container.Text("No photos").Style(Typography.Normal);
            return;
        }

        container
            //.DebugArea("Photos")
            .Grid(grid =>
        {
            grid.Spacing(5);
            grid.Columns(2);

            model.Photos.ForEach(x =>
            {
                var image = new ImagePlaceholder()
                {
                    ImagePath = x.Path,
                    Observation = x.Observation
                };
                grid
                .Item()
                .AlignCenter()
                .ScaleToFit()
                .Component(image);
            });
        });
    }
}
