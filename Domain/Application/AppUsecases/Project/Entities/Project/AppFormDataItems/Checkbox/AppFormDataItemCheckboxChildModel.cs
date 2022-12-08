using System.Collections.Generic;

using AppUsecases.Entities.AppFormDataItems.Text;

namespace AppUsecases.Entities.AppFormDataItems.Checkbox;
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
