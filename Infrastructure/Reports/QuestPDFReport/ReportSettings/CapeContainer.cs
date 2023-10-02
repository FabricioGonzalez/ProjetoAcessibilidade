using XmlDatasource.Solution.DTO;

namespace QuestPDFReport.ReportSettings;

public class CapeContainer
{
    public CompanyInfoItem CompanyInfo
    {
        get;
        set;
    }
    public ManagementCompanyInfo ManagerInfo
    {
        get;
        set;
    }

    public IEnumerable<PartnerItem> Partners
    {
        get;
        set;
    }
}