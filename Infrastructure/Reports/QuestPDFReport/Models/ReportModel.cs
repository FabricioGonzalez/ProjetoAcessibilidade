using XmlDatasource.Solution.DTO;

namespace QuestPDFReport.Models;

public class ReportModel : IReport
{
    public List<ReportSection> Sections
    {
        get;
        set;
    } = new();

    public List<ReportPhoto> Photos
    {
        get;
        set;
    } = new();

    public string CompanyLogo
    {
        get;
        set;
    }

    public byte[] LogoData
    {
        get;
        set;
    }

    public FieldsContainer HeaderFields
    {
        get;
        set;
    } = new();

    public string Title
    {
        get;
        set;
    }

    public CompanyInfoItem CompanyInfo
    {
        get;
        set;
    }

    public string StandardLaw
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

public class NestedReportModel : IReport
{
    public List<ReportLocationGroup> Locations
    {
        get;
        set;
    } = new();

    public List<ReportPhoto> Photos
    {
        get;
        set;
    } = new();

    public string CompanyLogo
    {
        get;
        set;
    }

    public byte[] LogoData
    {
        get;
        set;
    }

    public string Conclusion
    {
        get;
        set;
    }

    public FieldsContainer HeaderFields
    {
        get;
        set;
    } = new();

    public string Title
    {
        get;
        set;
    }

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

    public string StandardLaw
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

public interface IReport
{
    public string Title
    {
        get;
        set;
    }

    public CompanyInfoItem CompanyInfo
    {
        get;
        set;
    }

    public string StandardLaw
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

    public FieldsContainer HeaderFields
    {
        get;
        set;
    }
}

public class FieldsContainer
{
    public ReportHeaderField Local
    {
        get;
        set;
    }

    public ReportHeaderField Data
    {
        get;
        set;
    }

    public ReportHeaderField Empresa
    {
        get;
        set;
    }

    public ReportHeaderField Responsavel
    {
        get;
        set;
    }

    public ReportHeaderField Email
    {
        get;
        set;
    }

    public ReportHeaderField Telefone
    {
        get;
        set;
    }

    public ReportHeaderField Gerenciadora
    {
        get;
        set;
    }

    public ReportHeaderField Uf
    {
        get;
        set;
    }

    public ReportHeaderField Revisao
    {
        get;
        set;
    }
}