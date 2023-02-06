using Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
public class AppFormDataItemCheckboxModel : IAppFormDataItemContract
{
    public string Topic
    {
        get; set;
    }
    public AppFormDataType Type { get; set; } = AppFormDataType.Checkbox;
    public List<AppFormDataItemCheckboxChildModel> Children
    {
        get; set;
    }
}
