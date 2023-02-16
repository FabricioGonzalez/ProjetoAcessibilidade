using Core.Entities.Solution.Project.AppItem.DataItems.Text;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
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
