using Common.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;

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
        if (Model is ReportSectionGroup reportSectionGroup)
        {
            container
                .Section(reportSectionGroup.Title)
                .Decoration(decoration =>
                {
                    _ = decoration
                        .Before()
                        .PaddingBottom(5)
                        .ShowOnce()
                        .Text(reportSectionGroup.Title)
                        .Style(Typography.Headline);

                    decoration
                        .Content()
                        .Border(0.75f)
                        .BorderColor(Colors.Grey.Medium)
                        .Column(col =>
                        {
                            _ = reportSectionGroup.Parts.IterateOn(item =>
                            {
                                col.Item()
                                    .Section($"{reportSectionGroup.Title} - {item.Title}")
                                    .Column(column =>
                                    {
                                        column.Item()
                                            .Decoration(contentDecoration =>
                                            {
                                                contentDecoration
                                                    .Before()
                                                    .ShowOnce()
                                                    .Column(column =>
                                                    {
                                                        _ = column
                                                            .Item()
                                                            .PaddingLeft(8)
                                                            .Text(item.Title)
                                                            .Style(Typography.SubLine);
                                                    });

                                                contentDecoration
                                                    .Content()
                                                    .Column(
                                                        column =>
                                                        {
                                                            column
                                                                .Item()
                                                                .Column(items =>
                                                                {
                                                                    _ = item.Parts.IterateOn(item =>
                                                                    {
                                                                        items
                                                                            .Item()
                                                                            .ValueCell()
                                                                            .Column(partColumn =>
                                                                            {
                                                                                if (item is not ReportSectionTitle)
                                                                                {
                                                                                    if (item is ReportSectionText text)
                                                                                    {
                                                                                        partColumn
                                                                                            .Item()
                                                                                            .ValueCell()
                                                                                            .ExtendHorizontal()
                                                                                            .Row(row =>
                                                                                            {
                                                                                                row.RelativeItem(0.6f)
                                                                                                    .Text(item.Label);

                                                                                                _ = row.RelativeItem(
                                                                                                        0.3f)
                                                                                                    .Text(text.Text);
                                                                                                _ = row.RelativeItem(
                                                                                                        0.1f)
                                                                                                    .AlignRight()
                                                                                                    .Text(text
                                                                                                        .MeasurementUnit);
                                                                                            });
                                                                                    }

                                                                                    if (item is ReportSectionCheckbox
                                                                                     checkboxes)
                                                                                    {
                                                                                        partColumn
                                                                                            .Item()
                                                                                            .ExtendHorizontal()
                                                                                            .Row(checkboxRow =>
                                                                                            {
                                                                                                checkboxRow
                                                                                                    .RelativeItem(0.4f)
                                                                                                    .Text(item.Label);

                                                                                                checkboxRow
                                                                                                    .RelativeItem(0.6f)
                                                                                                    .Row(
                                                                                                        innerCheckboxRow =>
                                                                                                        {
                                                                                                            checkboxes
                                                                                                                .Checkboxes
                                                                                                                .Select(
                                                                                                                    CheckboxItemComponent =>
                                                                                                                        new
                                                                                                                            CheckboxItemComponent(
                                                                                                                                CheckboxItemComponent))
                                                                                                                .IterateOn(
                                                                                                                    CheckboxItemComponent =>
                                                                                                                        innerCheckboxRow
                                                                                                                            .RelativeItem()
                                                                                                                            .AlignCenter()
                                                                                                                            .Background(
                                                                                                                                CheckboxItemComponent
                                                                                                                                    .item
                                                                                                                                    .IsValid
                                                                                                                                    ? Colors
                                                                                                                                        .Red
                                                                                                                                        .Lighten1
                                                                                                                                    : Colors
                                                                                                                                        .White)
                                                                                                                            .Component(
                                                                                                                                CheckboxItemComponent));
                                                                                                        });
                                                                                            });
                                                                                    }
                                                                                }
                                                                            });
                                                                    });
                                                                });

                                                            column
                                                                .Item()
                                                                .Background(Colors.Yellow.Medium)
                                                                .Column(items =>
                                                                {
                                                                    items
                                                                        .Item()
                                                                        .Element(x =>
                                                                            ObservationElement(x, item.Observation));
                                                                });

                                                            if (item.Images.Any())
                                                            {
                                                                column
                                                                    .Item()
                                                                    .Column(items =>
                                                                    {
                                                                        items
                                                                            .Item()
                                                                            .Element(x =>
                                                                                PhotosElement(x, item.Images));
                                                                    });
                                                            }
                                                        });
                                            });
                                        column
                                            .Item()
                                            .Element(x => LawItemsElement(x, item.Laws));
                                    });
                            });
                        });
                });
        }
    }

    private void ObservationElement(
        IContainer x
        , IEnumerable<ObservationSectionElement> observation
    ) => x.ExtendHorizontal().Column(column =>
    {
        _ = column.Item().LabelCell().Text("Observações");

        _ = observation.IterateOn(item =>
        {
            _ = column.Item().ValueCell().Text(item.Observation);
        });
    });


    private void PhotosElement(
        IContainer container
        , IEnumerable<ImageSectionElement> models
    ) =>
        container
            /*.DebugArea("Photos")*/
            .Grid(grid =>
            {
                grid.Spacing(5);
                grid.Columns(2);

                _ = models.IterateOn(item =>
                {
                    var image = new ReportImage
                    {
                        ImagePath = item.ImagePath, Observation = item.Observation
                    };
                    grid
                        .Item()
                        .AlignCenter()
                        .Component(image);
                });
            });

    private void LawItemsElement(
        IContainer container
        , IEnumerable<LawSectionElement> models
    ) =>
        container
            .Padding(2)
            .ExtendHorizontal()
            .Column(column =>
            {
                _ = models.IterateOn(item =>
                {
                    _ = column.Item().LabelCell().Text(item.LawId);

                    _ = column.Item().ValueCell().Text(item.LawContent);
                });
            });
}