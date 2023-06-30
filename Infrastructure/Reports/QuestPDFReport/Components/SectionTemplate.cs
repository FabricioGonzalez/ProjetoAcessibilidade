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
                    _ = decoration
                        .Before()
                        .PaddingBottom(value: 5)
                        .ShowOnce()
                        .Text(text: reportSectionGroup.Title)
                        .Style(style: Typography.Headline);

                    decoration
                        .Content()
                        .Border(value: 0.75f)
                        .BorderColor(color: Colors.Grey.Medium)
                        .Column(col =>
                        {

                            _ = reportSectionGroup.Parts.IterateOn((item) =>
                            {
                                col.Item()
                                .Section(sectionName: $"{reportSectionGroup.Title} - {item.Title}")
                                .Column(column =>
                                {
                                    column.Item()
                                     .Decoration(handler: contentDecoration =>
                                     {
                                         contentDecoration
                                          .Before()
                                          .ShowOnce()
                                          .Column(column =>
                                   {
                                       _ = column
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
                                                   _ = item.Parts.IterateOn((item) =>
                                                   {
                                                       items
                                                           .Item()
                                                           .ValueCell()
                                                           .Column(handler: partColumn =>
                                                           {
                                                               _ = partColumn
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
                                                                       frame.Row(row =>
                                                                       {
                                                                           _ = row.RelativeItem(0.9f).Text(text: text.Text);
                                                                           _ = row.RelativeItem(0.1f).AlignRight().Text(text: text.MeasurementUnit);

                                                                       });
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
                                                                               _ = checkboxes
                                                                          .Checkboxes
                                                                          .Select(item =>
                                                                          new CheckboxItemComponent(item))
                                                                          .IterateOn(item =>
                                                                          checkboxRow
                                                                          .RelativeItem()
                                                                          .AlignCenter()
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
                });
        }
    }

    private void ObservationElement(
        IContainer x
        , IEnumerable<ObservationSectionElement> observation
    ) => x.ExtendHorizontal().Column(handler: column =>
    {
        _ = column.Item().LabelCell().Text("Observações");

        _ = observation.IterateOn((item) =>
        {
            _ = column.Item().ValueCell().Text(text: item.Observation);
        });

    });


    private void PhotosElement(
        IContainer container
        , IEnumerable<ImageSectionElement> models
    )
    {
        container
            /*.DebugArea("Photos")*/
            .Grid(handler: grid =>
            {
                grid.Spacing(value: 5);
                grid.Columns(value: 2);

                _ = models.IterateOn((item) =>
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
            .ExtendHorizontal()
            .Column(handler: column =>
        {
            _ = models.IterateOn((item) =>
            {
                _ = column.Item().LabelCell().Text(item.LawId);

                _ = column.Item().ValueCell().Text(item.LawContent);
            });

        });

    }
}