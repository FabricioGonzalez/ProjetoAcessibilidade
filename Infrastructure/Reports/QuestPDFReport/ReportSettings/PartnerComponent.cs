using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

using QuestPDFReport.Components;

using XmlDatasource.Solution.DTO;

namespace QuestPDFReport.ReportSettings;

internal class PartnerComponent : IComponent
{
    private readonly PartnerItem _partnerItem;

    public PartnerComponent(
        PartnerItem partnerItem
    )
    {
        _partnerItem = partnerItem;
    }

    public void Compose(
        IContainer container
    ) =>
        container
            .Width(182)
            .Column(col =>
            {
                col.Item()
                    .Height(100)
                    .ScaleHorizontal(1.1f)
                    .Element(it =>
                    {
                        it.Component(new CapeImage { ImagePath = _partnerItem.PartnerLogo });
                    });

                col.Item().Height(2);
                col.Item().AlignCenter().Text(_partnerItem.WebSite);
            });
}