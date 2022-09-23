using System.Collections.Generic;

using AppUsecases.Entities.AppFormDataItems.Text;

namespace AppWinui.AppCode.Project.States.ProjectItems.Checkbox;
public class AppFormDataItemCheckboxChildModel
{
    public string Topic
    {
        get; set;
    }
    public ICollection<AppOptionModel> Options
    {
        get; set;
    }
    public ICollection<AppFormDataItemTextModel> TextItems
    {
        get; set;
    }
}
