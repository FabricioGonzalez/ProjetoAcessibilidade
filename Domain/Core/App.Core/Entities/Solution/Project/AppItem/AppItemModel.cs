using Core.Entities.Solution.Project.AppItem.DataItems;

namespace Core.Entities.Solution.Project.AppItem;
public class AppItemModel
{

    public string ItemName
    {
        get; set;
    }
    public IList<IAppFormDataItemContract> FormData
    {
        get; set;
    }
    public IList<AppLawModel> LawList
    {
        get; set;
    }
}
