using System.Text;
using Common.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;

namespace QuestPDFReport.Components;

public class SectionTemplate : IComponent
{
    private readonly bool strechImages;

    public SectionTemplate(
        ReportLocationGroup model
    )
    {
        Model = model;
    }

    public SectionTemplate(ReportLocationGroup model, bool strechImages) : this(model)
    {
        this.strechImages = strechImages;
    }

    public ReportLocationGroup Model
    {
        get;
        set;
    }

    public void Compose(
        IContainer container
    ) =>
        container
            .Table(root =>
            {
                root.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                });
                Model
                    .Groups
                    .IterateOn(group =>
                    {
                        root.Cell()
                            .Border(0.6f)
                            .BorderColor(Colors.Grey.Darken1)
                            .Section($"{Model.Title} - {group.Title}")
                            .Decoration(decoration =>
                            {
                                GroupItemHeader(group, decoration);

                                decoration
                                    .Content()
                                    .Border(0.75f)
                                    .BorderColor(Colors.Grey.Medium)
                                    .Column(col =>
                                    {
                                        group.Parts.IterateOn(item =>
                                        {
                                            col.Item()
                                                .Section($"{Model.Title} - {group.Title} - {item.Title}")
                                                .Column(column =>
                                                {
                                                    column
                                                        .Item()
                                                        /*.PaddingVertical(2)*/
                                                        .Decoration(contentDecoration =>
                                                        {
                                                            FormBodyTitle(item, contentDecoration);

                                                            contentDecoration
                                                                .Content()
                                                                .PaddingVertical(2)
                                                                .Column(
                                                                    formBody =>
                                                                    {
                                                                        FormParts(item, formBody);

                                                                        ObservationItem(item, formBody);
                                                                    });
                                                        });
                                                    column
                                                        .Item()
                                                        .Element(x =>
                                                            LawItemsElement(x, item.Laws));

                                                    ImageItems(item, column, strechImages);
                                                });
                                        });
                                    });
                            });
                    });
            });

    private void FormParts(ReportSection item, ColumnDescriptor formBody) =>
        formBody
            .Item()
            .Column(items =>
            {
                item
                    .Parts
                    .IterateOn(formItem =>
                    {
                        items
                            .Item()
                            .ValueCell()
                            .Column(partColumn =>
                            {
                                if (formItem is not
                                    ReportSectionTitle)
                                {
                                    if (formItem is
                                        ReportSectionText text)
                                    {
                                        FormTextItem(partColumn, formItem.Label, text.Text, text.MeasurementUnit);
                                    }

                                    if (formItem is
                                        ReportSectionCheckbox
                                        checkboxes)
                                    {
                                        FormCheckboxItem(formItem, partColumn, checkboxes);
                                    }
                                }
                            });
                    });
            });

    private void ImageItems(ReportSection item, ColumnDescriptor column, bool StrechImages = false)
    {
        if (item.Images.Any())
        {
            column
                .Item()
                .ExtendHorizontal()
                .Grid(grid =>
                {
                    grid.Spacing(5);
                    grid.Columns(2);

                    item
                        .Images
                        .Where(it =>
                            !string.IsNullOrWhiteSpace(it.ImagePath))
                        .IterateOn(item =>
                        {
                            var image = new ReportImage(160, 160, StrechImages)
                            {
                                ImagePath = item.ImagePath,
                                Observation = item.Observation
                            };
                            grid
                                .Item()
                                .Element(
                                    it => it.Component(image));
                        });
                });
        }
    }

    private void ObservationItem(ReportSection item, ColumnDescriptor formBody)
    {
        if (item.Observation.Any())
        {
            formBody
                .Item()
                .Column(items =>
                {
                    items
                        .Item()
                        .Element(x =>
                            ObservationElement(x, item.Observation));
                });
        }
    }

    private void FormCheckboxItem(ReportSectionElement item, ColumnDescriptor partColumn,
        ReportSectionCheckbox checkboxes)
    {
        partColumn
            .Item()
            .ExtendHorizontal()
            .Row(checkboxRow =>
            {
                checkboxRow
                    .RelativeItem(0.90f)
                    .Text(item.Label)
                    .SemiBold()
                    .WrapAnywhere();

                checkboxRow
                    .AutoItem()
                    .AlignRight()
                    .AlignMiddle()
                    .Row(
                        innerCheckboxRow =>
                        {
                            checkboxes
                                .Checkboxes
                                .Select(
                                    CheckboxItemComponent =>
                                        new CheckboxItemComponent(CheckboxItemComponent))
                                .IterateOn(CheckboxItemComponent =>
                                    innerCheckboxRow
                                        .AutoItem()
                                        /*.Width(50)*/
                                        .Element(it => it.Component(CheckboxItemComponent)));
                        });

                partColumn.Item().Column(col =>
                {
                    checkboxes.TextItems.IterateOn(it =>
                    {
                        FormTextItem(col, it.Label, it.Text, it.MeasurementUnit, hasBorder: true);
                    });
                });
            });
    }

    private void FormTextItem(ColumnDescriptor partColumn, string label, string text, string measurementUnit,
        bool hasBorder = false)
    {
        var element = partColumn
            .Item()
            .ExtendHorizontal();
        if (hasBorder)
        {
            element = element.BorderBottom(0.1f);
        }

        element.Row(row =>
        {
            row
                .AutoItem()
                .PaddingHorizontal(2)
                .AlignMiddle()
                .Text(label)
                .SemiBold()
                .WrapAnywhere(false);

            row
                .RelativeItem()
                .PaddingRight(4)
                .Text(text)
                .WrapAnywhere(false);

            row
                .AutoItem()
                .AlignRight()
                .AlignMiddle()
                .Text(measurementUnit)
                .FontSize(10)
                .SemiBold()
                .FontColor(Colors.Black);
        });
    }


    private void FormBodyTitle(ReportSection item, DecorationDescriptor contentDecoration) =>
        contentDecoration
            .Before()
            .ShowOnce()
            .Column(formBody =>
            {
                formBody
                    .Item()
                    .PaddingLeft(8)
                    .Text(item.Title)
                    .Style(Typography.SubLine)
                    .FontColor(Colors.Black);
            });

    private void GroupItemHeader(ReportSectionGroup group, DecorationDescriptor decoration) =>
        decoration
            .Before()
            .PaddingBottom(4)
            .PaddingHorizontal(2)
            .Border(0.1f)
            .BorderColor(Colors.Grey.Lighten1)
            .ShowOnce()
            .Text(group.Title)
            .Style(Typography.Headline)
            .FontColor(Colors.Red.Medium);

    private void ObservationElement(
        IContainer x
        , IEnumerable<ObservationSectionElement> observation
    ) => x.ExtendHorizontal().PaddingVertical(4).Column(column =>
    {
        column.Item().LabelCell().Text("Observações");
        var sb = new StringBuilder();

        observation.IterateOn(item =>
        {
            sb.AppendLine(item.Observation);
        });
        column.Item().Background(Colors.Yellow.Medium).Text(sb.ToString());
    });


    private void PhotosElement(
        IContainer container
        , IEnumerable<ImageSectionElement> models
    ) =>
        container
            .Grid(grid =>
            {
                grid.Spacing(5);
                grid.Columns(2);

                _ = models.IterateOn(item =>
                {
                    var image = new ReportImage(160, 160)
                    {
                        ImagePath = item.ImagePath,
                        Observation = item.Observation
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
            .ExtendHorizontal()
            .Border(0.2f)
            .BorderColor(Colors.Grey.Darken1)
            .PaddingVertical(4)
            .Column(col =>
            {
                col.Item().LabelCell().Text("Legislação").SemiBold();
                col.Item().Column(column =>
                {
                    models.IterateOn(item =>
                    {
                        column.Item()
                            .LabelCell()
                            .Text(item.LawId)
                            .FontColor(Colors.Red.Medium);

                        column.Item()
                            .Background(Colors.LightBlue.Lighten2)
                            .Border(0.2f)
                            .BorderColor(Colors.Grey.Darken1)
                            .PaddingHorizontal(2)
                            .Text(item.LawContent)
                            .SemiBold();
                    });
                });
            });
}