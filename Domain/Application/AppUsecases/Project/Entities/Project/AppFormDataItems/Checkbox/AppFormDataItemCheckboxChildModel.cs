using System.Collections.Generic;

using AppUsecases.Project.Entities.Project.AppFormDataItems.Text;

namespace AppUsecases.Project.Entities.Project.AppFormDataItems.Checkbox;
public class AppFormDataItemCheckboxChildModel
{
    public string Topic
    {
        get; set;
    }
    public IList<AppOptionModel> Options
    {
        get; set;
    }
    public IList<AppFormDataItemTextModel> TextItems
    {
        get; set;
    }
}
