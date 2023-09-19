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

            column.Item().AlignCenter().Text(Cape.ManagerInfo.ReportDate.ToString("dd/MM/yyyy"));

            column.Item().Height(110);

            foreach (var capeItem in Cape.Partners.Chunk(2))
            {
                column.Item()
                    .Row(row =>
                    {
                        if (capeItem.Length > 0)
                            row.RelativeItem(0.4f)
                               /*.AlignCenter()*/
                               .AlignRight()
                               .Element(ele =>
                                   ele.Component(new PartnerComponent(capeItem[0])));

                        row.RelativeColumn(0.1f).Text("");

                        if (capeItem.Length > 1)
                            row.RelativeItem(0.4f)
                                   /*.AlignCenter()*/
                                   .AlignLeft()
                                   .Element(ele =>
                                       ele.Component(new PartnerComponent(capeItem[1])));
                    });
            }
        });
}