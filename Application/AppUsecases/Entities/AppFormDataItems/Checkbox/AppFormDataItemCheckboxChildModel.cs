using System.Collections.Generic;

using AppUsecases.Entities.AppFormDataItems.Text;

namespace AppUsecases.Entities.AppFormDataItems.Checkbox;
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
