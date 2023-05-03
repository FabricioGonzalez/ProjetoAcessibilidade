using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;
using SkiaSharp;

namespace QuestPDFReport.Components;

public class SectionTemplate : IComponent
{
    public SectionTemplate(
        IReportSection model
    )
    {
        Model = model;
    }

    public IReportSection Model
    {
        get;
        set;
    }

    public void Compose(
        IContainer container
    )
    {
        if (Model is ReportSection reportSection)
        {
            container
                .EnsureSpace()
                .Decoration(handler: decoration =>
                {
                    decoration
                        .Before()
                        .PaddingBottom(value: 5)
                        .Text(text: Model.Title)
                        .Style(style: Typography.Headline);

                    decoration
                        .Content()
                        .Section(sectionName: reportSection.Id)
                        .Border(value: 0.75f)
                        .BorderColor(color: Colors.Grey.Medium)
                        .Column(handler: column =>
                        {
                            foreach (var part in reportSection.Parts)
                            {
                                column
                                    .Item()
                                    .EnsureSpace(minHeight: 25)
                                    .Column(handler: column =>
                                    {
                                        column
                                            .Item()
                                            .LabelCell()
                                            .ExtendHorizontal()
                                            .Text(text: part.Label);

                                        if (part is not ReportSectionTitle)
                                        {
                                            var frame = column
                                                .Item()
                                                .ValueCell();

                                            if (part is ReportSectionText text)
                                            {
                                                frame
                                                    .ShowEntire()
                                                    .Text(text: text.Text);
                                            }

                                            if (part is ReportSectionCheckbox checkboxes)
                                            {
                                                frame
                                                    .Element(handler: x =>
                                                        MapCheckboxes(container: x, checkboxes: checkboxes));
                                            }

                                            if (part is ReportSectionPhotoContainer photos)
                                            {
                                                frame
                                                    .Element(handler: x => PhotosElement(container: x, model: photos));
                                            }

                                            if (part is ReportSectionObservation observation)
                                            {
                                                frame
                                                    .Background(color: Colors.Yellow.Medium)
                                                    .Element(handler: x =>
                                                        ObservationElement(x: x, observation: observation));
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
                .Section(sectionName: reportSectionGroup.Id)
                .Decoration(handler: decoration =>
                {
                    decoration
                        .Before()
                        .PaddingBottom(value: 5)
                        .ShowOnce()
                        .Text(text: reportSectionGroup.Title)
                        .Style(style: Typography.Headline);

                    decoration
                        .Content()
                        .Border(value: 0.75f)
                        .BorderColor(color: Colors.Grey.Medium)
                        .Decoration(handler: contentDecoration =>
                        {
                            foreach (var part in reportSectionGroup.Parts)
                            {
                                contentDecoration
                                    .Content()
                                    .Section(sectionName: part.Id)
                                    .Column(
                                        handler: column =>
                                        {
                                            column
                                                .Item()
                                                .PaddingLeft(value: 8)
                                                .ShowOnce()
                                                .Text(text: part.Title)
                                                .Style(style: Typography.SubLine);
                                            column.Item().Column(handler: items =>
                                            {
                                                foreach (var sectionPart in part.Parts)
                                                {
                                                    items
                                                        .Item()
                                                        .ValueCell()
                                                        .EnsureSpace(minHeight: 25)
                                                        .Column(handler: partColumn =>
                                                        {
                                                            partColumn
                                                                .Item()
                                                                .LabelCell()
                                                                .ExtendHorizontal()
                                                                .Text(text: sectionPart.Label);

                                                            if (sectionPart is not ReportSectionTitle)
                                                            {
                                                                var frame = partColumn
                                                                    .Item()
                                                                    .ValueCell();

                                                                if (sectionPart is ReportSectionText text)
                                                                {
                                                                    frame
                                                                        .ShowEntire()
                                                                        .Text(text: text.Text);
                                                                }

                                                                if (sectionPart is ReportSectionCheckbox checkboxes)
                                                                {
                                                                    frame
                                                                        .Element(handler: x =>
                                                                            MapCheckboxes(container: x,
                                                                                checkboxes: checkboxes));
                                                                }

                                                                if (sectionPart is ReportSectionPhotoContainer photos)
                                                                {
                                                                    frame
                                                                        .Element(handler: x =>
                                                                            PhotosElement(container: x, model: photos));
                                                                }

                                                                if (sectionPart is ReportSectionObservation observation)
                                                                {
                                                                    frame
                                                                        .Background(color: Colors.Yellow.Medium)
                                                                        .Element(handler: x =>
                                                                            ObservationElement(x: x,
                                                                                observation: observation));
                                                                }
                                                            }
                                                        });
                                                }
                                            });
                                        });
                            }
                        });
                });
        }
    }

    private void ObservationElement(
        IContainer x
        , ReportSectionObservation observation
    ) =>
        x
            .Text(text: observation.Observation)
            .Style(style: Typography.Normal);

    private void MapTitle(
        IContainer container
        , string title
    ) =>
        container.ShowEntire().Column(handler: column =>
        {
            column.Item().Text(text: title).Style(style: Typography.Normal);
        });

    private void MapCheckboxes(
        IContainer container
        , ReportSectionCheckbox checkboxes
    ) =>
        container.ShowEntire().Column(handler: column =>
        {
            column.Spacing(value: 5);
            column.Item().Row(handler: row =>
            {
                foreach (var item in checkboxes.Checkboxes)
                {
                    row.Spacing(value: 5);

                    row.ConstantItem(size: 16)
                        .Layers(handler: layers =>
                        {
                            layers.Layer().Canvas(handler: (
                                canvas
                                , size
                            ) =>
                            {
                                DrawRoundedRectangle(color: Colors.White, isStroke: false);
                                DrawRoundedRectangle(color: Colors.Black, isStroke: true);

                                if (item.IsChecked)
                                {
                                    DrawLine(color: Colors.Black, isStroke: true, fromX: 6, fromY: 4, toX: 10, toY: 12);
                                    DrawLine(color: Colors.Black, isStroke: true, fromX: 9.80f, fromY: 12.0f, toX: 15.5f
                                        , toY: -1f);
                                }


                                void DrawLine(
                                    string color
                                    , bool isStroke
                                    , float fromX
                                    , float fromY
                                    , float toX
                                    , float toY
                                )
                                {
                                    using var paint = new SKPaint
                                    {
                                        Color = SKColor.Parse(hexString: color), IsStroke = isStroke, StrokeWidth = 1,
                                        IsAntialias = true
                                    };

                                    canvas.DrawLine(p0: new SKPoint(x: fromX, y: fromY), p1: new SKPoint(x: toX, y: toY)
                                        , paint: paint);
                                }

                                void DrawRoundedRectangle(
                                    string color
                                    , bool isStroke
                                )
                                {
                                    using var paint = new SKPaint
                                    {
                                        Color = SKColor.Parse(hexString: color), IsStroke = isStroke, StrokeWidth = 1,
                                        IsAntialias = true
                                    };

                                    canvas.DrawRoundRect(x: 4, y: 2, w: 12, h: 12, rx: 2, ry: 4, paint: paint);
                                }
                            });

                            layers
                                .PrimaryLayer()
                                /* .Text("Sample text")
                                                                                                                                                                                                                                                                          .FontSize(16).FontColor(Colors.Blue.Darken2).SemiBold()*/
                                ;
                        });

                    row.AutoItem().Text(text: item.Value);
                }
            });
        });

    private void MapElement(
        IContainer container
        , ReportSectionMap model
    )
    {
        if (model.Location == null)
        {
            container.Text(text: "No location provided");
            return;
        }

        container
            .ShowEntire()
            .Column(handler: column =>
            {
                column
                    .Spacing(value: 5);

                column
                    .Item()
                    .MaxWidth(value: 250)
                    .AspectRatio(ratio: 4 / 3f)
                    .Component<ImagePlaceholder>();

                column
                    .Item()
                    .Text(text: model.Location.Format());
            });
    }

    private void PhotosElement(
        IContainer container
        , ReportSectionPhotoContainer model
    )
    {
        if (model.Photos.Count == 0)
        {
            container.Text(text: "No photos").Style(style: Typography.Normal);
            return;
        }

        container
            //.DebugArea("Photos")
            .Grid(handler: grid =>
            {
                grid.Spacing(value: 5);
                grid.Columns(value: 2);

                model.Photos.ForEach(action: x =>
                {
                    var image = new ImagePlaceholder
                    {
                        ImagePath = x.Path, Observation = x.Observation
                    };
                    grid
                        .Item()
                        .AlignCenter()
                        .ScaleToFit()
                        .Component(component: image);
                });
            });
    }
}