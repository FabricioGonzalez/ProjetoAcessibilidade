using System.Collections.Generic;

using AppUsecases.Contracts.Entity;

namespace AppUsecases.Project.Entities.Project;
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
