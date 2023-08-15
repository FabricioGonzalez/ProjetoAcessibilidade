using Common.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;

namespace QuestPDFReport.Components;

public class SectionTemplate : IComponent
{
    public SectionTemplate(
        ReportLocationGroup model
    )
    {
        Model = model;
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
            .Column(root =>
            {
                root.Item()
                    .Section(Model.Title)
                    .Text(Model.Title);
                Model.Groups.IterateOn(group =>
                {
                    root.Item()
                        .Section($"{Model.Title} - {group.Title}")
                        .Decoration(decoration =>
                        {
                            _ = decoration
                                .Before()
                                .PaddingBottom(5)
                                .ShowOnce()
                                .Text(group.Title)
                                .Style(Typography.Headline);

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
                                                column.Item()
                                                    .Decoration(contentDecoration =>
                                                    {
                                                        contentDecoration
                                                            .Before()
                                                            .ShowOnce()
                                                            .Column(formBody =>
                                                            {
                                                                formBody
                                                                    .Item()
                                                                    .PaddingLeft(8)
                                                                    .Text(item.Title)
                                                                    .Style(Typography.SubLine);
                                                            });

                                                        contentDecoration
                                                            .Content()
                                                            .Column(
                                                                formBody =>
                                                                {
                                                                    formBody
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
                                                                                        if (item is not
                                                                                             ReportSectionTitle)
                                                                                        {
                                                                                            if (item is
                                                                                                 ReportSectionText text)
                                                                                            {
                                                                                                partColumn
                                                                                                    .Item()
                                                                                                    .ValueCell()
                                                                                                    .ExtendHorizontal()
                                                                                                    .Row(row =>
                                                                                                    {
                                                                                                        row
                                                                                                            .RelativeItem(
                                                                                                                0.6f)
                                                                                                            .Text(item
                                                                                                                .Label);

                                                                                                        _ = row
                                                                                                            .RelativeItem(
                                                                                                                0.3f)
                                                                                                            .Text(text
                                                                                                                .Text);
                                                                                                        _ = row
                                                                                                            .RelativeItem(
                                                                                                                0.1f)
                                                                                                            .AlignRight()
                                                                                                            .Text(text
                                                                                                                .MeasurementUnit);
                                                                                                    });
                                                                                            }

                                                                                            if (item is
                                                                                                 ReportSectionCheckbox
                                                                                                 checkboxes)
                                                                                            {
                                                                                                partColumn
                                                                                                    .Item()
                                                                                                    .ExtendHorizontal()
                                                                                                    .Row(checkboxRow =>
                                                                                                    {
                                                                                                        checkboxRow
                                                                                                            .RelativeItem(
                                                                                                                0.4f)
                                                                                                            .Text(item
                                                                                                                .Label);

                                                                                                        checkboxRow
                                                                                                            .RelativeItem(
                                                                                                                0.6f)
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
                                                                                                                                    .Component(
                                                                                                                                        CheckboxItemComponent));
                                                                                                                });
                                                                                                    });
                                                                                            }
                                                                                        }
                                                                                    });
                                                                            });
                                                                        });

                                                                    formBody
                                                                        .Item()
                                                                        .Background(Colors.Yellow.Medium)
                                                                        .Column(items =>
                                                                        {
                                                                            items
                                                                                .Item()
                                                                                .Element(x =>
                                                                                    ObservationElement(x: x
                                                                                        , observation: item
                                                                                            .Observation));
                                                                        });
                                                                });
                                                    });
                                                column
                                                    .Item()
                                                    .Element(x => LawItemsElement(container: x, models: item.Laws));
                                                if (item.Images.Any())
                                                {
                                                    column
                                                        .Item()
                                                        .Element(x =>
                                                            PhotosElement(container: x
                                                                , models: item.Images));
                                                }
                                            });
                                    });
                                });
                        });
                });
            });

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