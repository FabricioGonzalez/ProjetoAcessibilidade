using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDFReport.Components;

namespace QuestPDFReport.ReportSettings;

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
            column.Item().Height(70);

            column.Item().Height(120).AlignCenter().Element(it =>
                it.Component(new CapeImage { ImagePath = Cape.CompanyInfo.LogoPath }));

            column.Item().Height(60);

            column.Item().AlignCenter().Text(Cape.CompanyInfo.NomeEmpresa);

            column.Item().Height(20);

            column.Item().AlignCenter().Text("RELATÓRIO DE NÃO  CONFORMIDADE ACESSIBILIDADE");

            column.Item().Height(20);

            column.Item().AlignCenter().Text(Cape.CompanyInfo.Data.ToString("dd/MM/yyyy"));

            column.Item().Height(110);

            foreach (var capeItem in Cape.Partners.Chunk(2))
            {
                column.Item()
                    .Row(row =>
                    {
                        foreach (var cape in capeItem)
                        {
                            row.RelativeItem(0.5f)
                                .AlignCenter()
                                .Element(ele =>
                                    ele.Component(new PartnerComponent(cape)));
                        }
                    });
            }
        });
}