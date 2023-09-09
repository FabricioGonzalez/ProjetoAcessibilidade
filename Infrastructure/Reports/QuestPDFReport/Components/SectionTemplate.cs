using System.Text;

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
            .Table(root =>
            {
                root.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                });
                Model.Groups.IterateOn(group =>
                {
                    root.Cell()
                        .Border(0.6f)
                        .BorderColor(Colors.Grey.Darken1)
                        .Section($"{Model.Title} - {group.Title}")
                        .Decoration(decoration =>
                        {
                            decoration
                                .Before()
                                .PaddingBottom(5)
                                .PaddingHorizontal(2)
                                .Border(0.1f)
                                .BorderColor(Colors.Grey.Lighten1)
                                .ShowOnce()
                                .Text(group.Title)
                                .Style(Typography.Headline)
                                .FontColor(Colors.Red.Medium);

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
                                                                    .Style(Typography.SubLine)
                                                                    .FontColor(Colors.Black);
                                                                ;
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

                                                                                                        row
                                                                                                            .AutoItem()
                                                                                                            .PaddingRight(
                                                                                                                4)
                                                                                                            .Text(text
                                                                                                                .Text);
                                                                                                        row
                                                                                                            .AutoItem()
                                                                                                            .AlignRight()
                                                                                                            .Text(text
                                                                                                                .MeasurementUnit)
                                                                                                            .Style(
                                                                                                                Typography
                                                                                                                    .SubLine)
                                                                                                            .FontSize(
                                                                                                                10)
                                                                                                            .FontColor(
                                                                                                                Colors
                                                                                                                    .Black);
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
                                                                    if (item.Observation.Any())
                                                                        formBody
                                                                            .Item()
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

                                                if (item.Images.Any() 
                                                && item.Images.All(it => !string.IsNullOrWhiteSpace(it.ImagePath)))
                                                {
                                                    column
                                                        .Item()
                                                        .ExtendHorizontal()
                                                        .MaxHeight(120)
                                                        .Grid(grid =>
                                                        {
                                                            grid.Spacing(5);
                                                            grid.Columns(2);

                                                            item.Images.IterateOn(item =>
                                                            {
                                                                var image = new ReportImage
                                                                {
                                                                    ImagePath = item.ImagePath
                                                                    ,
                                                                    Observation = item.Observation
                                                                };
                                                                grid
                                                                    .Item()
                                                                    .AlignCenter()
                                                                    .Element(it => it.Component(image));
                                                            });
                                                        });
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
        column.Item().LabelCell().Text("Observações");
        var sb = new StringBuilder();

        observation.IterateOn(item =>
        {
            sb.AppendLine(item.Observation);
        });
        column.Item().ValueCell().Background(Colors.Yellow.Medium).Text(sb.ToString());
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
        .Column(col =>
        {
            col.Item().LabelCell().Text("Legislação").Weight(FontWeight.SemiBold);
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
                    .PaddingHorizontal(2)
                    .Text(item.LawContent)
                    .Weight(FontWeight.SemiBold);
                });
            });
        });

}