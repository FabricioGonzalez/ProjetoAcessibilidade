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
                .Section(sectionName: reportSectionGroup.Title)
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
                        .Column(column =>
                        {

                            reportSectionGroup.Parts.IterateOn((item) =>
                            {
                                column.Item()
                                      .Section(sectionName: $"{reportSectionGroup.Title} - {item.Title}")
                                      .Decoration(handler: contentDecoration =>
                                {
                                    contentDecoration
                                    .Before()
                                    .ShowOnce()
                                    .Column(column =>
                                    {
                                        column
                                        .Item()
                                        .PaddingLeft(value: 8)
                                        .Text(text: item.Title)
                                        .Style(style: Typography.SubLine);
                                    });

                                    contentDecoration
                                        .Content()
                                        .Column(
                                            handler: column =>
                                            {
                                                column
                                                .Item()
                                                .Column(handler: items =>
                                                {
                                                    item.Parts.IterateOn((item) =>
                                                    {
                                                        items
                                                            .Item()
                                                            .ValueCell()
                                                            .Column(handler: partColumn =>
                                                            {
                                                                partColumn
                                                                    .Item()
                                                                    .LabelCell()
                                                                    .ExtendHorizontal()
                                                                    .Text(text: item.Label);

                                                                if (item is not ReportSectionTitle)
                                                                {
                                                                    var frame = partColumn
                                                                        .Item()
                                                                        .ValueCell()
                                                                        .ExtendHorizontal();

                                                                    if (item is ReportSectionText text)
                                                                    {
                                                                        frame.Text(text: text.Text);
                                                                    }

                                                                    if (item is ReportSectionCheckbox checkboxes)
                                                                    {
                                                                        frame
                                                                        .AlignTop()
                                                                        .Column(column =>
                                                                        {
                                                                            column
                                                                            .Item()
                                                                            .Row(checkboxRow =>
                                                                            {
                                                                                checkboxes
                                                                           .Checkboxes
                                                                           .Select(item =>
                                                                           new CheckboxItemComponent(item))
                                                                           .IterateOn(item =>
                                                                           checkboxRow
                                                                           .RelativeItem()
                                                                           .MinHeight(20)
                                                                           .MaxHeight(40)
                                                                           .Component(item));
                                                                            });
                                                                        });
                                                                    }
                                                                }
                                                            });
                                                    });
                                                });

                                                column
                                              .Item()
                                              .Column(handler: items =>
                                              {
                                                  column
                                                  .Item()
                                                  .Element(x => ObservationElement(x, item.Observation));
                                              });
                                                if (item.Images.Count() > 0)
                                                {
                                                    column
                                                   .Item()
                                                   .Column(handler: items =>
                                                   {
                                                       column
                                                       .Item()
                                                       .Element(x => PhotosElement(x, item.Images));
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
        }
    }

    private void ObservationElement(
        IContainer x
        , IEnumerable<ObservationSectionElement> observation
    ) => x.Column(handler: column =>
    {
        column.Item().Text("Observações");

        observation.IterateOn((item) =>
        {
            column.Item().Text(text: item.Observation);
        });

    });


    private void PhotosElement(
        IContainer container
        , IEnumerable<ImageSectionElement> models
    )
    {
        container
            .DebugArea("Photos")
            .Grid(handler: grid =>
            {
                grid.Spacing(value: 5);
                grid.Columns(value: 2);

                models.IterateOn((item) =>
        {
            var image = new ImagePlaceholder
            {
                ImagePath = item.ImagePath,
                Observation = item.Observation
            };
            grid
                .Item()
                .AlignCenter()
                .Component(component: image);
        });
            });

    }

    private void LawItemsElement(
        IContainer container
        , IEnumerable<LawSectionElement> models
    )
    {
        container
            .Padding(2)
            .Column(handler: column =>
        {
            models.IterateOn((item) =>
            {
                column.Item().Text(item.LawId);

                column.Item().Text(item.LawContent);
            });

        });

    }
}