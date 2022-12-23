using System.Collections.Generic;

using AppUsecases.Project.Contracts.Entity;
using AppUsecases.Project.Enums;

namespace AppUsecases.Project.Entities.Project.AppFormDataItems.Checkbox;
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
