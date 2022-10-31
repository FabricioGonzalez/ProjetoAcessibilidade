using System.Collections.Generic;

using AppUsecases.Contracts.Entity;
using AppUsecases.Entities.AppFormDataItems.Text;

namespace AppUsecases.Entities.AppFormDataItems.Checkbox;
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
