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
        /*if (Model is ReportSection reportSection)
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
        }*/

        if (Model is ReportSectionGroup reportSectionGroup)
        {
            container
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
                                                .Text(text: part.Title)
                                                .Style(style: Typography.SubLine);
                                            column
                                            .Item()
                                            .Column(handler: items =>
                                            {
                                                foreach (var sectionPart in part.Parts)
                                                {
                                                    items
                                                        .Item()
                                                        .ValueCell()
                                                        .EnsureSpace()
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
                                                                    .ValueCell()
                                                                    .ExtendHorizontal();

                                                                if (sectionPart is ReportSectionText text)
                                                                {
                                                                    frame
                                                                        .ShowEntire()
                                                                        .Text(text: text.Text);
                                                                }

                                                                if (sectionPart is ReportSectionCheckbox checkboxes)
                                                                {
                                                                    frame
                                                                    .AlignTop()
                                                                    .Column(column =>
                                                                    {
                                                                        column
                                                                        .Item()
                                                                        .EnsureSpace(25)
                                                                        .Row(checkboxRow =>
                                                                        {
                                                                            checkboxes
                                                                       .Checkboxes
                                                                       .Select(item =>
                                                                       new CheckboxItemComponent(item))
                                                                       .IterateOn(item =>
                                                                       checkboxRow
                                                                       .RelativeItem()
                                                                       .Height(20)
                                                                       .Component(item));
                                                                        });
                                                                    });
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
                        ImagePath = x.Path,
                        Observation = x.Observation
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