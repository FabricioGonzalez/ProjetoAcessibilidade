using System.Collections.Generic;

using App.Core.Entities.Solution.Project.AppItem.DataItems.Text;

namespace App.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
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
