using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Images;

namespace Core.Entities.Solution.Project.AppItem;

public class AppItemModel
{
    public string Id
    {
        get;
        set;
    }

    public string ItemName
    {
        get;
        set;
    }

    public string TemplateName
    {
        get;
        set;
    }

    public IEnumerable<IAppFormDataItemContract> FormData
    {
        get;
        set;
    }

    public IEnumerable<ObservationModel> Observations
    {
        get;
        set;
    }
    public IEnumerable<ImagesItem> Images
    {
        get;
        set;
    }
    public IEnumerable<AppLawModel> LawList
    {
        get;
        set;
    }
}