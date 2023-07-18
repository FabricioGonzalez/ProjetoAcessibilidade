using Common.Linq;
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
            Title = Model.Title, DocumentLayoutExceptionThreshold = 1000000000
        };

    public void Compose(
        IDocumentContainer container
    ) =>
        container
            .Page(page =>
            {
                page
                    .DefaultTextStyle(Typography.Normal);

                page
                    .MarginVertical(5);

                page
                    .MarginHorizontal(15);

                page
                    .Size(PageSizes.A4);

                page
                    .Header()
                    .SkipOnce()
                    .Element(ComposeHeader);

                page
                    .Content()
                    .Element(ComposeContent);

                page
                    .Footer()
                    .AlignCenter()
                    .SkipOnce()
                    .Text(text =>
                    {
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
            });

    private void ComposeHeader(
        IContainer container
    ) =>
        container.Column(column =>
        {
            column
                .Item().PaddingTop(-10)
                .Text(Model.Title)
                .Style(Typography.Title);

            column.Item()
                .PaddingVertical(15)
                .Border(1f)
                .BorderColor(Colors.Grey.Lighten1)
                .ExtendHorizontal();

            column
                .Item()
                .Border(0.5f)
                .Grid(grid =>
                {
                    grid.Columns(4);
                    grid.Spacing(5);

                    foreach (var field in Model.HeaderFields)
                    {
                        grid.Item().Column(columnHeader =>
                        {
                            columnHeader.Item().Text(field.Label);

                            columnHeader.Item().Text(field.Value);
                        });
                    }
                });

            column
                .Item()
                .PaddingVertical(5)
                .LineHorizontal(1)
                .LineColor(Colors.Grey.Medium);

            column
                .Item()
                .Container()
                .Background(Colors.LightBlue.Lighten2)
                .Text(Model.StandardLaw)
                .Style(Typography.Headline)
                .FontColor(Colors.Black);
        });

    private void ComposeContent(
        IContainer container
    )
    {
        if (Model is NestedReportModel nestedModel)
        {
            container
                .PaddingVertical(20)
                .Column(column =>
                {
                    column.Spacing(20);

                    column
                        .Item()
                        .Padding(0)
                        .ExtendHorizontal()
                        .Component(new CapeComponent(new CapeContainer { CompanyLogo = Model.CompanyLogo }));

                    column.Item().PageBreak();

                    column
                        .Item()
                        .Component(new TableOfContentsTemplate(nestedModel
                            .Locations
                            .ToList()));
                    
                    column.Item().PageBreak();
                    
                    nestedModel.Locations.IterateOn(section =>
                    {
                        column
                            .Item()
                            .Section(section.Title)
                            .Component(new SectionTemplate(section));
                    });
                });
        }
    }
}

internal class CapeComponent : IComponent
{
    public CapeComponent(
        CapeContainer cape
    )
    {
        Cape = cape;
    }

    public CapeContainer Cape
    {
        get;
        set;
    }

    public void Compose(
        IContainer container
    ) =>
        container.Column(column =>
        {
            column.Spacing(20);
            column.Item().Height(280).ExtendHorizontal().AlignTop()
                .Component(new CapeImage { ImagePath = Cape.CompanyLogo });
            column.Item().AlignCenter().Column(row =>
            {
                row.Item().Text(Cape.CompanyName);
                row.Item().Text("RELATÓRIO DE NÃO  CONFORMIDADE ACESSIBILIDADE");
                row.Item().Text(DateTime.Now.ToString("ddddd, dd/MM/yyyy"));
            });
            column.Item().AlignBottom();
        });
}

public class CapeContainer
{
    public string CompanyName
    {
        get;
        set;
    } = "ARPA";

    public string CompanyLogo
    {
        get;
        set;
    }
}