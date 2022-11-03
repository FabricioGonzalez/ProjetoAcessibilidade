using AppUsecases.Contracts.Entity;
using AppUsecases.Project.Enums;

namespace AppWinui.AppCode.Project.States.ProjectItems.Checkbox;
public class AppFormDataItemCheckboxModel : IAppFormDataItemContract
{
    public string Topic
    {
        get; set;
    }
    public AppFormDataTypeEnum Type { get; set; } = AppFormDataTypeEnum.Checkbox;
    public List<AppFormDataItemCheckboxChildModel> Children
    {
        get; set;
    }
}
