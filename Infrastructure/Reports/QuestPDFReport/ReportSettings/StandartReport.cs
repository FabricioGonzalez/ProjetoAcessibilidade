using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDFReport.Components;
using QuestPDFReport.Models;

namespace QuestPDFReport.ReportSettings;

public class StandardReport : IDocument
{
    public StandardReport(
        IReport model
    )
    {
        Model = model;
    }

    private IReport Model
    {
        get;
    }

    public DocumentMetadata GetMetadata() =>
        new()
        {
            Title = Model.Title, DocumentLayoutExceptionThreshold = 5000
        };

    public void Compose(
        IDocumentContainer container
    ) =>
        container
            .Page(handler: page =>
            {
                page
                    .DefaultTextStyle(textStyle: Typography.Normal);

                page
                    .MarginVertical(value: 10);

                page
                    .MarginHorizontal(value: 30);

                page
                    .Size(pageSize: PageSizes.A4);

                page
                    .Header()
                    .Element(handler: ComposeHeader);

                page
                    .Content()
                    .Element(handler: ComposeContent);

                page
                    .Footer()
                    .AlignCenter()
                    .Text(content: text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(text: " / ");
                        text.TotalPages();
                    });
            });

    private void ComposeHeader(
        IContainer container
    ) =>
        container.Column(handler: column =>
        {
            column.Item().PaddingTop(value: -10).Text(text: Model.Title).Style(style: Typography.Title);

            column.Item().ShowOnce().PaddingVertical(value: 15).Border(value: 1f)
                .BorderColor(color: Colors.Grey.Lighten1).ExtendHorizontal();

            column.Item().ShowOnce().Border(value: 0.5f).Grid(handler: grid =>
            {
                grid.Columns(value: 4);
                grid.Spacing(value: 5);

                foreach (var field in Model.HeaderFields)
                {
                    grid.Item().Column(handler: column =>
                    {
                        column.Item().Text(text: field.Label);

                        column.Item().Text(text: field.Value);
                    });
                }
            });

            column.Item().PaddingVertical(value: 5).LineHorizontal(size: 1).LineColor(value: Colors.Grey.Medium);

            column.Item().Container().Background(color: Colors.LightBlue.Lighten2).Text(text: Model.StandardLaw)
                .Style(style: Typography.Headline)
                .FontColor(value: Colors.Black);
        });

    private void ComposeContent(
        IContainer container
    )
    {
        if (Model is NestedReportModel nestedModel)
        {
            container.PaddingVertical(value: 20).Column(handler: column =>
            {
                column.Spacing(value: 20);

                column
                    .Item()
                    .Component(component: new TableOfContentsTemplate(sections: nestedModel
                        .Sections
                        .Cast<IReportSection>()
                        .ToList()));

                column
                    .Item()
                    .PageBreak();

                foreach (var section in nestedModel.Sections)
                {
                    column
                        .Item()
                        .Section(sectionName: section.Title)
                        .Component(component: new SectionTemplate(model: section));
                }


                if (nestedModel.Photos.Count > 0)
                {
                    column
                        .Item()
                        .PageBreak();
                    column
                        .Item()
                        .Section(sectionName: "Photos");

                    foreach (var photo in nestedModel.Photos)
                    {
                        column
                            .Item()
                            .Component(component: new PhotoTemplate(model: photo));
                    }
                }
            });
        }

        /*
        if (Model is ReportModel reportModel)
        {
            container.PaddingVertical(value: 20).Column(handler: column =>
            {
                column.Spacing(value: 20);

                column
                    .Item()
                    .Component(component: new TableOfContentsTemplate(
                        sections: reportModel
                            .Sections
                            .Cast<IReportSection>()
                            .ToList()));

                column
                    .Item()
                    .PageBreak();

                foreach (var section in reportModel.Sections)
                {
                    column
                        .Item()
                        .Section(sectionName: section.Id)
                        .Component(component: new SectionTemplate(model: section));
                }


                if (reportModel.Photos.Count > 0)
                {
                    column
                        .Item()
                        .PageBreak();
                    column
                        .Item()
                        .Section(sectionName: "Photos");

                    foreach (var photo in reportModel.Photos)
                    {
                        column
                            .Item()
                            .Component(component: new PhotoTemplate(model: photo));
                    }
                }
            });
        }*/
    }
}