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
                    .Content()
                    .Element(ComposeContent);

                page
                    .Footer()
                    .AlignCenter()
                    .SkipOnce()
                    .Text(text =>
                    {
                        text.Span("Página ");

                        text.CurrentPageNumber();
                        /*text.Span(" / ");
                        text.TotalPages();*/
                    });
            });

    private void ComposeHeader(
        IContainer container
    ) =>
        container
            .Column(column =>
            {
                column
                    .Item()
                    .Border(0.5f)
                    .ExtendHorizontal()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header
                                .Cell()
                                .ColumnSpan(6)
                                .AlignCenter()
                                .Text(Model.Title)
                                .Style(Typography.SubLine).FontColor(Colors.Black).FontSize(14);
                        });
                        table.Cell().LabelCell().Text("");

                        table.Cell().ValueCell().Text("");

                        table
                            .Cell()
                            .Row(1)
                            .Column(2)
                            .ColumnSpan(3)
                            .LabelCell()
                            .Text(Model.HeaderFields.Local.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(2)
                            .Column(2)
                            .ColumnSpan(3)
                            .ValueCell()
                            .Text(Model.HeaderFields.Local.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(1)
                            .Column(5)
                            .LabelCell()
                            .Text(Model.HeaderFields.Uf.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(2)
                            .Column(5)
                            .ValueCell()
                            .Text(Model.HeaderFields.Uf.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(1)
                            .Column(6)
                            .ColumnSpan(2)
                            .LabelCell()
                            .Text(Model.HeaderFields.Data.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(2)
                            .Column(6)
                            .ColumnSpan(2)
                            .ValueCell()
                            .Text(Model.HeaderFields.Data.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(3)
                            .Column(1)
                            .LabelCell()
                            .Text(Model.HeaderFields.Revisao.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(4)
                            .Column(1)
                            .ValueCell()
                            .Text(Model.HeaderFields.Revisao.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(3)
                            .Column(2)
                            .ColumnSpan(4)
                            .LabelCell()
                            .Text(Model.HeaderFields.Empresa.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(4)
                            .Column(2)
                            .ColumnSpan(4)
                            .ValueCell()
                            .Text(Model.HeaderFields.Empresa.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(3)
                            .Column(6)
                            .ColumnSpan(2)
                            .LabelCell()
                            .Text(Model.HeaderFields.Empresa.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(4)
                            .Column(6)
                            .ColumnSpan(2)
                            .ValueCell()
                            .Text(Model.HeaderFields.Empresa.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(5)
                            .ColumnSpan(7)
                            .Text("");

                        table
                            .Cell()
                            .Row(6)
                            .Column(1)
                            .ColumnSpan(3)
                            .LabelCell()
                            .Text(Model.HeaderFields.Responsavel.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(7)
                            .Column(1)
                            .ColumnSpan(3)
                            .ValueCell()
                            .Text(Model.HeaderFields.Empresa.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(6)
                            .Column(4)
                            .ColumnSpan(2)
                            .LabelCell()
                            .Text(Model.HeaderFields.Email.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(7)
                            .Column(4)
                            .ColumnSpan(2)
                            .ValueCell()
                            .Text(Model.HeaderFields.Email.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(6)
                            .Column(6)
                            .LabelCell()
                            .Text(Model.HeaderFields.Telefone.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(7)
                            .Column(6)
                            .ValueCell()
                            .Text(Model.HeaderFields.Telefone.Value)
                            .FontSize(8);

                        table
                            .Cell()
                            .Row(6)
                            .Column(7)
                            .LabelCell()
                            .Text(Model.HeaderFields.Gerenciadora.Label)
                            .Style(Typography.SubLine)
                            .FontColor(Colors.Black)
                            .FontSize(10);

                        table
                            .Cell()
                            .Row(7)
                            .Column(7)
                            .ValueCell()
                            .Text(Model.HeaderFields.Gerenciadora.Value)
                            .FontSize(8);
                    });

                column
                    .Item()
                    .PaddingVertical(1);

                column
                    .Item()
                    .Container()
                    .Background(Colors.LightBlue.Lighten2)
                    .Border(0.5f)
                    .BorderColor(Colors.Grey.Medium)
                    .Padding(4)
                    .Text(Model.StandardLaw)
                    .Style(Typography.Headline)
                    .FontColor(Colors.Grey.Darken4);
            });

    private void ComposeContent(
        IContainer container
    )
    {
        if (Model is NestedReportModel nestedModel)
        {
            container
                .Column(column =>
                {
                    column
                        .Item()
                        .ExtendHorizontal()
                        .Element(it => it
                            .Component(new CapeComponent(new CapeContainer
                                { CompanyInfo = Model.CompanyInfo, Partners = Model.Partners })));

                    column.Item().PageBreak();

                    column
                        .Item()
                        .Element(it => it
                            .Component(new TableOfContentsTemplate(nestedModel
                                .Locations
                                .ToList())));

                    column.Item().PageBreak();

                    nestedModel.Locations.IterateOn(section =>
                    {
                        column.Item()
                            .Section(section.Title)
                            .ExtendHorizontal()
                            .Column(dcor =>
                            {
                                dcor.Item()
                                    .Border(0.5f)
                                    .ExtendHorizontal()
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Header(header =>
                                        {
                                            header
                                                .Cell()
                                                .ColumnSpan(6)
                                                .AlignCenter()
                                                .Text(Model.Title)
                                                .Style(Typography.SubLine).FontColor(Colors.Black).FontSize(14);
                                        });
                                        table.Cell().LabelCell().Text("");

                                        table.Cell().ValueCell().Text("");

                                        table
                                            .Cell()
                                            .Row(1)
                                            .Column(2)
                                            .ColumnSpan(3)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Local.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(2)
                                            .Column(2)
                                            .ColumnSpan(3)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Local.Value)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(1)
                                            .Column(5)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Uf.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(2)
                                            .Column(5)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Uf.Value)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(1)
                                            .Column(6)
                                            .ColumnSpan(2)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Data.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(2)
                                            .Column(6)
                                            .ColumnSpan(2)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Data.Value)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(3)
                                            .Column(1)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Revisao.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(4)
                                            .Column(1)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Revisao.Value)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(3)
                                            .Column(2)
                                            .ColumnSpan(4)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Empresa.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(4)
                                            .Column(2)
                                            .ColumnSpan(4)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Empresa.Value)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(3)
                                            .Column(6)
                                            .ColumnSpan(2)
                                            .LabelCell()
                                            .Text("Edifício")
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(4)
                                            .Column(6)
                                            .ColumnSpan(2)
                                            .ValueCell()
                                            .Text(section.Title)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(5)
                                            .ColumnSpan(7)
                                            .Text("");

                                        table
                                            .Cell()
                                            .Row(6)
                                            .Column(1)
                                            .ColumnSpan(3)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Responsavel.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(7)
                                            .Column(1)
                                            .ColumnSpan(3)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Empresa.Value)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(6)
                                            .Column(4)
                                            .ColumnSpan(2)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Email.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(7)
                                            .Column(4)
                                            .ColumnSpan(2)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Email.Value)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(6)
                                            .Column(6)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Telefone.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(7)
                                            .Column(6)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Telefone.Value)
                                            .FontSize(8);

                                        table
                                            .Cell()
                                            .Row(6)
                                            .Column(7)
                                            .LabelCell()
                                            .Text(Model.HeaderFields.Gerenciadora.Label)
                                            .Style(Typography.SubLine)
                                            .FontColor(Colors.Black)
                                            .FontSize(10);

                                        table
                                            .Cell()
                                            .Row(7)
                                            .Column(7)
                                            .ValueCell()
                                            .Text(Model.HeaderFields.Gerenciadora.Value)
                                            .FontSize(8);
                                    });

                                dcor
                                    .Item()
                                    .PaddingVertical(1);

                                dcor
                                    .Item()
                                    .Container()
                                    .Background(Colors.LightBlue.Lighten2)
                                    .Border(0.5f)
                                    .BorderColor(Colors.Grey.Medium)
                                    .Padding(4)
                                    .Text(Model.StandardLaw)
                                    .Style(Typography.Headline)
                                    .FontColor(Colors.Grey.Darken4);
                            });

                        column.Item()
                            .Element(it =>
                                it.Component(new SectionTemplate(section)));

                        column.Item().PageBreak();
                    });

                    column.Item().Section("conclusao").LabelCell().Text("Conclusão");
                    column.Item().ValueCell().Text(nestedModel.Conclusion);
                });
        }
    }
}